using EmployeeService.Application.Cqrs.Commands.EmployeeForm;
using EmployeeService.Entities.Forms;    // Employee entity namespace
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;

namespace EmployeeService.Cqrs.Commands;
public sealed class CreateTimeTableHandler(IRepository<Employee> repo)
    : ICommandHandler<CreateTimeTableCommand, int>
{
    public async Task<int> HandleAsync(CreateTimeTableCommand c, CancellationToken ct)
    {
        var e = new Employee { Name = c.Name, Gender = c.Gender };
        await repo.AddAsync(e);
        await repo.SaveChangesAsync();
        return e.Id;
    }
}