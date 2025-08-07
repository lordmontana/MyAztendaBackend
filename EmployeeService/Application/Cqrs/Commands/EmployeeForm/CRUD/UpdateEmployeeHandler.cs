
using EmployeeService.Entities.Forms;
using EmployeeService.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
using Shared.Web.Exceptions;

namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;

public sealed class UpdateEmployeeHandler(
        IRepository<Employee> repo, ApplicationDbContext db)
    : EmployeeSaveHandler<UpdateEmployeeCommand>(repo, db)
{
    protected override async Task<(ActionKind, Employee)> PrepareEntityAsync(
        UpdateEmployeeCommand cmd, CancellationToken ct)
    {
        var entity = await db.Employee.FindAsync(new object[] { cmd.Id }, ct)
                     ?? throw new DomainRuleException("Employee not found");
        return (ActionKind.Update, entity);
    }

    protected override void Map(UpdateEmployeeCommand cmd, Employee e, ActionKind action)
    {
        var d = cmd.Employee;
        e.Name = d.Name;
        e.Gender = d.Gender;
        e.Email = d.Email;
    }

    protected override int GetId(Employee e) => e.Id;
}