
using EmployeeService.Entities.Forms;
using EmployeeService.Persistence;
using Shared.Repositories.Abstractions;

namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;

public sealed class CreateEmployeeHandler(
        IRepository<Employee> repo, ApplicationDbContext db)
    : EmployeeSaveHandler<CreateEmployeeCommand>(repo, db)
{
    protected override Task<(ActionKind, Employee)> PrepareEntityAsync(
        CreateEmployeeCommand cmd, CancellationToken ct) =>
        Task.FromResult((ActionKind.Create, new Employee()));

    protected override void Map(CreateEmployeeCommand cmd, Employee e, ActionKind action)
    {
        var d = cmd.Employee;
        e.Name = d.Name;
        e.Gender = d.Gender;
        e.Email = d.Email;
    }

    protected override int GetId(Employee e) => e.Id;
}
