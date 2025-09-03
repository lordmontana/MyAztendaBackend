// Application/Cqrs/Commands/EmployeeForm/EmployeeSaveHandler.cs
using EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;
using EmployeeService.Domain.Entities.Forms;
using EmployeeService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Abstractions;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
using Shared.Web.Exceptions;

namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm;

public abstract class EmployeeSaveHandler<TCommand>(IRepository<Employee> repo,ApplicationDbContext db)
    : SaveEntityHandler<Employee, TCommand>(repo) where TCommand : ICommand<int>, IEmployeePayload
{
    // ── shared BUSINESS RULES (runs for both Create & Update) ──
    protected override async Task BeforeSaveAsync(TCommand cmd,Employee e,ActionKind action,CancellationToken ct)
    {

        var test2  = cmd.Employee.Email;


        bool taken = await db.Employee
                              .AsNoTracking()
                              .AnyAsync(emp => emp.Email == test2 &&
                                               emp.Id != GetId(e), ct);
        if (taken)
            throw new DomainRuleException($"Email '{test2}' already exists.");


        if (action== ActionKind.Create)
        {
            // additional rules for Create action
            if (await db.Employee.AnyAsync(emp => emp.Name == e.Name, ct))
                throw new DomainRuleException($"Name '{e.Name}' already exists.");
        }
        if (action == ActionKind.Update)
        {
            // additional rules for Update action
            if (await db.Employee.AnyAsync(emp => emp.Name == e.Name && emp.Id != e.Id, ct))
                throw new DomainRuleException($"Name '{e.Name}' already exists.");
        }
    }

    // ── shared AFTER-SAVE hook ──
    protected override Task AfterSaveAsync(Employee e, ActionKind action, CancellationToken ct)
    {
        // e.g. publish domain event, send mail…
        // if (action == ActionKind.Create) _bus.Publish(new EmployeeCreated(e.Id));
        return Task.CompletedTask;
    }



}
