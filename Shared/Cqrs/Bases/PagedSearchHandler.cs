// Shared/Cqrs/Bases/PagedSearchHandler.cs
using Shared.Cqrs.Abstractions;
using Shared.Dtos;
using Shared.Filtering;
using Shared.Repositories.Abstractions;
using System.Linq.Expressions;

namespace Shared.Cqrs.Bases;

/// <summary>
/// Generic handler for queries derived from <see cref="PagedSearchQuery{TEntity,TDto}"/>.
/// Child classes supply a mapper and default ordering.
/// </summary>
public abstract class PagedSearchHandler<TEntity, TDto, TQuery>
        : IQueryHandler<TQuery, PagedResult<TDto>>
        where TEntity : class
        where TQuery : PagedSearchQuery<TEntity, TDto>
{
    private readonly IRepository<TEntity> _repo;

    protected PagedSearchHandler(IRepository<TEntity> repo) => _repo = repo;

    /// Map an entity to its DTO.
    protected abstract Func<TEntity, TDto> Map { get; }

    /// Default order by column for the grid.
    protected abstract Expression<Func<TEntity, object>> OrderBy { get; }

    public async Task<PagedResult<TDto>> HandleAsync(TQuery q, CancellationToken ct)
    {
        var parser = ParserFactory.Get<TEntity>(q.Mode);
        var filters = parser.Parse(q.Filters);
        var result = await _repo.QueryAsync(q.Page, q.PageSize, OrderBy, true, filters);

        var list = result.Data.Select(Map).ToList();
        return new PagedResult<TDto>(list, result.Total);
    }
}
