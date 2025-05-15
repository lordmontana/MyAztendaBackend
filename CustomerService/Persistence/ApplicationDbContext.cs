using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // You can override OnModelCreating if you want to customize your database schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

}

