using Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Domain.Entities.Forms
{
    public class Employee : BaseEntity
    {

        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }

    }
}
