// DeleteEmployeeCommand.cs
using Shared.Cqrs.Abstractions;
namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;
public sealed record DeleteEmployeeCommand(int Id) : ICommand;
