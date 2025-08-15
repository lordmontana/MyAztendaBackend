using Shared.Cqrs.Abstractions;
using EmployeeService.DTOs;

public sealed record GetEmployeeByIdQuery(int Id) : IQuery<EmployeeDto?>;