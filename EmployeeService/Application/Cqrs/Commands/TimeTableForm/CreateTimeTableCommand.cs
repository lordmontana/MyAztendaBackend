using Shared.Cqrs.Abstractions;

namespace EmployeeService.Application.Cqrs.Commands.TimeTableForm;

public sealed record CreateTimeTableCommand(string Name, string Gender) : ICommand<int>;


