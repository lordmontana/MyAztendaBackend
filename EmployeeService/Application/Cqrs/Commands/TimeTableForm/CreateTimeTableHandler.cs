using EmployeeService.Application.Cqrs.Commands.TimeTableForm;
using EmployeeService.Domain.Entities.Forms;    // Employee entity namespace
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;

namespace EmployeeService.Application.Cqrs.Commands.TimeTableForm;
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