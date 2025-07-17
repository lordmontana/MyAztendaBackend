using Shared.Entities;

namespace EmployeeService.Entities
{
    public class TimeTable : BaseEntity
    {
        public int? EmployeeId { get; set; }        // FK → Employee
        public DateTime? WeekStartUtc { get; set; }
        public int? HoursMonday { get; set; }
        public int? HoursTuesday { get; set; }
        public int? HoursWednesday { get; set; }
        public int? HoursThursday { get; set; }
        public int? HoursFriday { get; set; }
        public int? HoursSaturday { get; set; }
        public int? HoursSunday { get; set; }
        public string? Notes { get; set; }
    }
}
