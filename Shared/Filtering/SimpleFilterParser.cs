using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Filtering;

/// <summary>
/// Parses a list of <see cref="FilterDto"/> objects into LINQ <see cref="Expression{Func{TEntity, bool}}"/>
/// filters for use in EF Core queries. Supports string, numeric, boolean, enum, and DateTime fields,
/// including nullable types, with a whitelist for filterable fields.
/// </summary>
/// <typeparam name="TEntity">The entity type to filter.</typeparam>
public sealed class SimpleFilterParser<TEntity> : IFilterParser<TEntity>
{
    /// <summary>
    /// Set of all public instance property names (case-insensitive) of TEntity for whitelisting allowed filters.
    /// </summary>
    private static readonly HashSet<string> _whitelist =
        typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Parses a sequence of <see cref="FilterDto"/>s into a set of filter expressions.
    /// </summary>
    /// <param name="filters">Collection of filters to apply.</param>
    /// <returns>Array of filter expressions suitable for LINQ Where calls.</returns>
    /// <exception cref="ArgumentException">Thrown if a field is not filterable or parsing fails.</exception>
    public Expression<Func<TEntity, bool>>[] Parse(IEnumerable<FilterDto> filters)
    {
        var list = new List<Expression<Func<TEntity, bool>>>();

        foreach (var f in filters.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            if (f.Field.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                // Special case: OR across all string properties
                list.Add(BuildOrAcrossAll(f.Value));
                continue;
            }

            if (!_whitelist.Contains(f.Field))
                throw new ArgumentException($"Field '{f.Field}' is not filterable.");

            list.Add(BuildPredicate(f.Field, f.Op ?? "eq", f.Value));
        }
        return list.ToArray();
    }

    /// <summary>
    /// Builds an expression for a single field, operation, and value.
    /// Supports: string, numeric, boolean, enum, and DateTime (including nullables).
    /// </summary>
    /// <param name="field">The field/property name on TEntity.</param>
    /// <param name="op">The operation (eq, neq, gt, gte, lt, lte, contains, starts, ends).</param>
    /// <param name="value">The value as string to compare.</param>
    /// <returns>A filter expression.</returns>
    /// <exception cref="ArgumentException">Thrown for unknown fields or parse errors.</exception>
    /// <exception cref="NotSupportedException">Thrown for unsupported property types.</exception>
    private static Expression<Func<TEntity, bool>> BuildPredicate(string field, string op, string value)
    {
        var param = Expression.Parameter(typeof(TEntity), "e");
        var propInfo = typeof(TEntity).GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (propInfo == null)
            throw new ArgumentException($"Property '{field}' not found in '{typeof(TEntity).Name}'.");

        var prop = Expression.Property(param, propInfo);
        var type = propInfo.PropertyType;
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        // Normalize op for comparison
        op = op?.ToLower() ?? "eq";

        // --- STRING ---
        if (underlyingType == typeof(string))
        {
            // Add a not-null check for string properties
            var notNull = Expression.NotEqual(prop, Expression.Constant(null, typeof(string)));
            var toLower = Expression.Call(prop, nameof(string.ToLower), Type.EmptyTypes);
            Expression expr = op switch
            {
                "eq" => Expression.Equal(toLower, Expression.Constant(value.ToLower())),
                "neq" => Expression.NotEqual(toLower, Expression.Constant(value.ToLower())),
                "starts" => Expression.Call(
                    typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), Type.EmptyTypes,
                    Expression.Property(null, typeof(EF), nameof(EF.Functions)), toLower,
                    Expression.Constant($"{value.ToLower()}%")),
                "ends" => Expression.Call(
                    typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), Type.EmptyTypes,
                    Expression.Property(null, typeof(EF), nameof(EF.Functions)), toLower,
                    Expression.Constant($"%{value.ToLower()}")),
                "contains" => Expression.Call(
                    typeof(DbFunctionsExtensions), nameof(DbFunctionsExtensions.Like), Type.EmptyTypes,
                    Expression.Property(null, typeof(EF), nameof(EF.Functions)), toLower,
                    Expression.Constant($"%{value.ToLower()}%")),
                _ => Expression.Equal(toLower, Expression.Constant(value.ToLower()))
            };
            var body = Expression.AndAlso(notNull, expr);
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }

        // --- BOOL (and Nullable<bool>) ---
        if (underlyingType == typeof(bool))
        {
            // Accepts 1/0/true/false (case-insensitive)
            bool parsed = value.Trim().ToLower() switch { "1" or "true" => true, _ => false };
            var constant = Expression.Constant(parsed, type);
            var body = op == "neq"
                ? Expression.NotEqual(prop, constant)
                : Expression.Equal(prop, constant);

            // For nullable, ensure HasValue
            if (Nullable.GetUnderlyingType(type) != null)
            {
                var hasValue = Expression.Property(prop, "HasValue");
                body = Expression.AndAlso(hasValue, body);
            }
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }

        // --- NUMERIC (and Nullable) ---
        if (underlyingType == typeof(int) || underlyingType == typeof(long) ||
            underlyingType == typeof(double) || underlyingType == typeof(decimal) ||
            underlyingType == typeof(float) || underlyingType == typeof(short))
        {
            object convValue;
            try
            {
                convValue = Convert.ChangeType(value, underlyingType);
            }
            catch
            {
                throw new ArgumentException($"Value '{value}' could not be parsed as {underlyingType.Name}.");
            }
            var constant = Expression.Constant(convValue, type);
            Expression body = op switch
            {
                "gt" => Expression.GreaterThan(prop, constant),
                "gte" => Expression.GreaterThanOrEqual(prop, constant),
                "lt" => Expression.LessThan(prop, constant),
                "lte" => Expression.LessThanOrEqual(prop, constant),
                "neq" => Expression.NotEqual(prop, constant),
                _ => Expression.Equal(prop, constant)
            };

            if (Nullable.GetUnderlyingType(type) != null)
            {
                var hasValue = Expression.Property(prop, "HasValue");
                body = Expression.AndAlso(hasValue, body);
            }
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }

        // --- ENUM (and Nullable<Enum>) ---
        if (underlyingType.IsEnum)
        {
            object parsed;
            if (int.TryParse(value, out var intVal))
                parsed = Enum.ToObject(underlyingType, intVal);
            else
                parsed = Enum.Parse(underlyingType, value, true);

            var constant = Expression.Constant(parsed, underlyingType);
            var compare = op == "neq"
                ? Expression.NotEqual(prop, constant)
                : Expression.Equal(prop, constant);

            if (Nullable.GetUnderlyingType(type) != null)
            {
                var hasValue = Expression.Property(prop, "HasValue");
                compare = Expression.AndAlso(hasValue, compare);
            }
            return Expression.Lambda<Func<TEntity, bool>>(compare, param);
        }

        // --- DATETIME (and Nullable<DateTime>) ---
        if (underlyingType == typeof(DateTime))
        {
            if (!DateTime.TryParse(value, out var dt))
                throw new ArgumentException($"Invalid date: '{value}'");
            var constant = Expression.Constant(dt, type);

            Expression body = op switch
            {
                "gt" => Expression.GreaterThan(prop, constant),
                "gte" => Expression.GreaterThanOrEqual(prop, constant),
                "lt" => Expression.LessThan(prop, constant),
                "lte" => Expression.LessThanOrEqual(prop, constant),
                "neq" => Expression.NotEqual(prop, constant),
                _ => Expression.Equal(prop, constant)
            };
            if (Nullable.GetUnderlyingType(type) != null)
            {
                var hasValue = Expression.Property(prop, "HasValue");
                body = Expression.AndAlso(hasValue, body);
            }
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }

        throw new NotSupportedException($"Type '{type.Name}' unsupported by parser.");
    }

    /// <summary>
    /// Builds a filter expression for searching a value across all string properties of TEntity
    /// (OR-ed together using SQL LIKE).
    /// </summary>
    /// <param name="value">The value to search for (case-insensitive).</param>
    /// <returns>An expression matching any string field containing the value.</returns>
    private static Expression<Func<TEntity, bool>> BuildOrAcrossAll(string value)
    {
        var param = Expression.Parameter(typeof(TEntity), "e");
        Expression? body = null;
        var pattern = Expression.Constant($"%{value.ToLower()}%");

        foreach (var p in _whitelist)
        {
            var propInfo = typeof(TEntity).GetProperty(p, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            // Only string properties are included in full-text search
            if (propInfo == null || (Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType) != typeof(string))
                continue;

            var propExpr = Expression.Property(param, propInfo);
            var notNull = Expression.NotEqual(propExpr, Expression.Constant(null, typeof(string)));
            var lower = Expression.Call(propExpr, nameof(string.ToLower), Type.EmptyTypes);

            Expression like = Expression.Call(
                typeof(DbFunctionsExtensions),
                nameof(DbFunctionsExtensions.Like),
                Type.EmptyTypes,
                Expression.Property(null, typeof(EF), nameof(EF.Functions)),
                lower,
                pattern);

            like = Expression.AndAlso(notNull, like);
            body = body is null ? like : Expression.OrElse(body, like);
        }

        // If there are no string fields, default to always-false
        body ??= Expression.Constant(false);
        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
}
