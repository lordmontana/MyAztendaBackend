namespace EmployeeService.DTOs
{
    public class EmployeeDto
    {
        public EmployeeDto( int id, string? name, string? gender,string? email)
        {
            Id = id;
            Name = name;
            Gender = gender;
            Email = email;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; } 
    }
}
