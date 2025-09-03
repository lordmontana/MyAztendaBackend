using EmployeeService.Domain.Entities.Forms;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;

public sealed class DeleteEmployeeHandler(IRepository<Employee> repo)
    : DeleteEntityHandler<Employee, DeleteEmployeeCommand>(repo)
{
    protected override int GetId(DeleteEmployeeCommand c) => c.Id;
}
