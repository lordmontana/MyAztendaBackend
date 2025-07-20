using Shared.Dtos;
using System.Linq.Expressions;

namespace Shared.Filtering
{

    /// <summary>Turns a list of DTO filters into 0-N LINQ predicates.</summary>
    public interface IFilterParser<TEntity>
    {
        Expression<Func<TEntity, bool>>[] Parse(IEnumerable<FilterDto> filters);
    }
}
