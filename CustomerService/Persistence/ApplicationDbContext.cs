using CustomerService.Entities;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerHistory> CustomerHistory { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // You can override OnModelCreating if you want to customize your database schema
        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Customer>().HasKey(c => c.Id);
            b.Entity<CustomerHistory>().HasKey(h => h.Id);

        
            base.OnModelCreating(b);
        }
    }

}

