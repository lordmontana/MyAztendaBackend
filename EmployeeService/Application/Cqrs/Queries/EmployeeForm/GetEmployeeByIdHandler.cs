// EmployeeService.Application.Cqrs.Queries.EmployeeForm/GetEmployeeByIdHandler.cs
using EmployeeService.DTOs;
using EmployeeService.Entities.Forms;
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;

public sealed class GetEmployeeByIdHandler(IRepository<Employee> repo)
    : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    public async Task<EmployeeDto?> HandleAsync(GetEmployeeByIdQuery q, CancellationToken ct)
    {
        var e = await repo.GetByIdAsync(q.Id);   // uses your Repository<T>.GetByIdAsync(int)
        if (e is null) return null;

        // map entity -> dto (adjust fields to your DTO)
        return new EmployeeDto(e.Id, e.Name, e.Gender, e.Email);
    }
}
