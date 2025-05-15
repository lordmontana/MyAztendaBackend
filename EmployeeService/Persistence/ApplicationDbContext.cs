using EmployeeService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // You can override OnModelCreating if you want to customize your database schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

}

