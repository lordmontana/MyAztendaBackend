using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities.Forms;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
using System.Linq.Expressions;

namespace EmployeeService.Application.Cqrs.Queries.EmployeeForm;

/// <summary>
/// Concrete handler inherits generic logic; only mapper & order column provided.
/// </summary>
public sealed class SearchEmployeesHandler(
        IRepository<Employee> repo)
    : PagedSearchHandler<Employee, EmployeeDto, SearchEmployeesQuery>(repo)
{
    protected override Func<Employee, EmployeeDto> Map =>
        e => new EmployeeDto(e.Id, e.Name, e.Gender,e.Email);

    protected override Expression<Func<Employee, object>> OrderBy => e => e.Name;
}
