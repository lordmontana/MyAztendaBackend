using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities.Forms;
using Shared.Cqrs.Abstractions;
using Shared.Cqrs.Bases;
using Shared.Dtos;

namespace EmployeeService.Application.Cqrs.Queries.EmployeeForm;

public sealed record SearchEmployeesQuery(
    int Page, int PageSize, string Mode, List<FilterDto> Filters)
    : PagedSearchQuery<Employee, EmployeeDto>(Page, PageSize, Mode, Filters);

