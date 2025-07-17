using Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Entities
{
    public class Employee : BaseEntity
    {

        public string? Name { get; set; }
        public string? Gender { get; set; }

    }
}
