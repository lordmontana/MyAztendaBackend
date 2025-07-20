using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Filtering;

public sealed class SimpleFilterParser<TEntity> : IFilterParser<TEntity>
{
    private static readonly HashSet<string> _whitelist =
        typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Select(p => p.Name)
                       .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public Expression<Func<TEntity, bool>>[] Parse(IEnumerable<FilterDto> filters)
    {
        var list = new List<Expression<Func<TEntity, bool>>>();

        foreach (var f in filters.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            if (f.Field.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                list.Add(BuildOrAcrossAll(f.Value));
            }
            else
            {
                if (!_whitelist.Contains(f.Field))
                    throw new ArgumentException($"Field '{f.Field}' is not filterable.");

                list.Add(BuildPredicate(f.Field, f.Value));
            }
        }
        return list.ToArray();
    }

    private static Expression<Func<TEntity, bool>> BuildPredicate(string field, string value)
    {
        var param = Expression.Parameter(typeof(TEntity), "e");
        var prop = Expression.Property(param, field);
        var t = prop.Type;

        Expression body;

        if (t == typeof(string))
        {
            var toLower = Expression.Call(prop, nameof(string.ToLower), Type.EmptyTypes);
            var pattern = Expression.Constant($"%{value.ToLower()}%");
            var like = Expression.Call(
                typeof(DbFunctionsExtensions),
                nameof(DbFunctionsExtensions.Like),
                Type.EmptyTypes,
                Expression.Property(null, typeof(EF), nameof(EF.Functions)),
                toLower,
                pattern);

            var notNull = Expression.NotEqual(prop, Expression.Constant(null, typeof(string)));
            body = Expression.AndAlso(notNull, like);
        }
        else if (t.IsEnum)
        {
            var parsed = Enum.Parse(t, value, true);
            body = Expression.Equal(prop, Expression.Constant(parsed));
        }
        else if (t == typeof(bool))
        {
            body = Expression.Equal(prop, Expression.Constant(bool.Parse(value)));
        }
        else if (t == typeof(int) || t == typeof(long) ||
                 t == typeof(double) || t == typeof(decimal))
        {
            var conv = Convert.ChangeType(value, t);
            body = Expression.Equal(prop, Expression.Constant(conv, t));
        }
        else
        {
            throw new NotSupportedException($"Type '{t.Name}' unsupported by simple parser.");
        }

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
    /* ──────────────────────────────────────────────────────────── */
    private static Expression<Func<TEntity, bool>> BuildOrAcrossAll(string value)
    {
        var param = Expression.Parameter(typeof(TEntity), "e");
        Expression? body = null;
        var pattern = Expression.Constant($"%{value.ToLower()}%");

        foreach (var p in _whitelist)
        {
            var propInfo = typeof(TEntity).GetProperty(p, BindingFlags.Public | BindingFlags.Instance)!;
            if (propInfo.PropertyType != typeof(string)) continue;     // OR across string cols only

            // LOWER(e.Prop) LIKE '%value%'
            var propExpr = Expression.Property(param, propInfo);
            var lower = Expression.Call(propExpr, nameof(string.ToLower), Type.EmptyTypes);
            var like = Expression.Call(
                                typeof(DbFunctionsExtensions),
                                nameof(DbFunctionsExtensions.Like),
                                Type.EmptyTypes,
                                Expression.Property(null, typeof(EF), nameof(EF.Functions)),
                                lower,
                                pattern);

            body = body is null ? like : Expression.OrElse(body, like);
        }

        if (body is null)                                             // no string columns
            body = Expression.Constant(false);

        return Expression.Lambda<Func<TEntity, bool>>(body, param);
    }
}
