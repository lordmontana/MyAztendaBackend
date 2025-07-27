//using EmployeeService.DTOs;
//using EmployeeService.Entities.Forms;
//using EmployeeService.Persistence;
//using Microsoft.EntityFrameworkCore;
//using Shared.Cqrs.Bases;
//using Shared.Repositories.Abstractions;
//using Shared.Web.Exceptions;
//
//namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm;
//
//public sealed class UpdateEmployeeHandler(
//        IRepository<Employee> repo,
//        ApplicationDbContext db)
//    : UpdateEntityHandler<Employee, UpdateEmployeeCommand>(repo, db)
//{
//    // a) fetch
//    protected override Task<Employee?> GetExistingAsync(UpdateEmployeeCommand c, CancellationToken ct)
//        => db.Employee.FindAsync(new object[] { c.Id }, ct).AsTask();
//    
//    protected override int GetId(Employee e) => e.Id;
//
//    // b) domain rules (email duplicate but ignore same record)
//    protected override async Task CheckBusinessRulesAsync(UpdateEmployeeCommand c, Employee entity, CancellationToken ct)
//    {
//        bool emailTaken = await db.Employee
//                                  .AnyAsync(e => e.Email == c.Employee.Email && e.Id != c.Id, ct);
//        if (emailTaken)
//            throw new DomainRuleException($"Email '{c.Employee.Email}' already exists.");
//      
//    }
//
//    // c) map changes (selectively or via AutoMapper)
//    protected override void Map(UpdateEmployeeCommand c, Employee e)
//    {
//        e.Name = c.Employee.Name;
//        e.Gender = c.Employee.Gender;
//        e.Email = c.Employee.Email;
//    }
//
//    // d) after-save side-effects
//    protected override Task AfterSaveAsync(Employee e, CancellationToken ct)
//        => Task.CompletedTask;            // send notification, etc.
//}

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