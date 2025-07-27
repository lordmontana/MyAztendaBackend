// Shared/Cqrs/Bases/PagedSearchQuery.cs
using Shared.Dtos;
using Shared.Filtering;
using Shared.Cqrs.Abstractions;

namespace Shared.Cqrs.Bases;

/// <summary>
/// Marker record for "paged search" queries.
/// </summary>
public abstract record PagedSearchQuery<TEntity, TDto>(
    int Page,
    int PageSize,
    string Mode,
    List<FilterDto> Filters)
    : IQuery<PagedResult<TDto>>
    where TEntity : class;
