// UpdateEmployeeCommand.cs
using EmployeeService.DTOs;
using Shared.Cqrs.Abstractions;

namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;

public sealed record UpdateEmployeeCommand(int Id, EmployeeDto Employee) : ICommand<int>, IEmployeePayload;
