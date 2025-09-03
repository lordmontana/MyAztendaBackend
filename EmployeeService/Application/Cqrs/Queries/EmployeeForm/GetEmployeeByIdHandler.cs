using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities.Forms;
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;

namespace EmployeeService.Application.Cqrs.Queries.EmployeeForm;

public sealed class GetEmployeeByIdHandler(IRepository<Employee> repo)
    : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    public async Task<EmployeeDto?> HandleAsync(GetEmployeeByIdQuery q, CancellationToken ct)
    {
        var e = await repo.GetByIdAsync(q.Id);
        if (e is null) return null;
        return new EmployeeDto(e.Id, e.Name, e.Gender, e.Email);
    }
}
