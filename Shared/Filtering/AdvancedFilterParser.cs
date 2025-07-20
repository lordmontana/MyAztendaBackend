// Shared/Filtering/AdvancedFilterParser.cs
using System.Linq.Expressions;
using System.Text;
using System.Linq.Dynamic.Core;
using Shared.Dtos;

namespace Shared.Filtering;

public sealed class AdvancedFilterParser<TEntity> : IFilterParser<TEntity>
{
    private static readonly ParsingConfig _cfg = new() { ResolveTypesBySimpleName = true };

    public Expression<Func<TEntity, bool>>[] Parse(IEnumerable<FilterDto> filters)
    {
        var sb = new StringBuilder();
        var args = new List<object>();
        int i = 0;

        foreach (var f in filters.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            if (i++ > 0) sb.Append(" AND ");
            sb.Append($"{f.Field}.ToLower().Contains(@{i - 1})");
            args.Add(f.Value.ToLower());
        }

        if (sb.Length == 0) return Array.Empty<Expression<Func<TEntity, bool>>>();

        var lambda = DynamicExpressionParser.ParseLambda<TEntity, bool>(
            _cfg, false, sb.ToString(), args.ToArray());

        return new[] { lambda };
    }
}
