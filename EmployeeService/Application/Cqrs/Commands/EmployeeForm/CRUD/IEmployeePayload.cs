using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD
{
    public interface IEmployeePayload
    {
        EmployeeDto Employee { get; }

    }
}
