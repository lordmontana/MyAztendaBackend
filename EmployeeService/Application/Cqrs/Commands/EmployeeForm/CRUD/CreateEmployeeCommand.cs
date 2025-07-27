using EmployeeService.DTOs;
using Shared.Cqrs.Abstractions;


namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD
{
    public sealed record CreateEmployeeCommand(EmployeeDto Employee)
        : ICommand<int>, IEmployeePayload;               // returns new Id
}