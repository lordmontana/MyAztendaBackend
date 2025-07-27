using EmployeeService.DTOs;
using Shared.Cqrs.Abstractions;
using Shared.Dtos;

namespace EmployeeService.Application.Cqrs.Queries.EmployeeForm
{
    public sealed record SearchCreateTimeTableQuery(
       int Page,
       int PageSize,
       string Mode,
       List<FilterDto> Filters)
       : IQuery<PagedResult<EmployeeDto>>;
}
