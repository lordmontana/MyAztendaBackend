using Shared.Cqrs.Abstractions;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Cqrs.Queries.EmployeeForm;

public sealed record GetEmployeeByIdQuery(int Id) : IQuery<EmployeeDto?>;
