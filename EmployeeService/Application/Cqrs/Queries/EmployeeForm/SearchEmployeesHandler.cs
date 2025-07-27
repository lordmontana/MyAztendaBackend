using EmployeeService.DTOs;
using EmployeeService.Entities.Forms;
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
        e => new EmployeeDto( e.Name, e.Gender,"");

    protected override Expression<Func<Employee, object>> OrderBy => e => e.Name;
}
