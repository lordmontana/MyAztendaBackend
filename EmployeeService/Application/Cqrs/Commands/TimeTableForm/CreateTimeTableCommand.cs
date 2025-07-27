using Shared.Cqrs.Abstractions;

namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm;

public sealed record CreateTimeTableCommand(string Name, string Gender) : ICommand<int>;


