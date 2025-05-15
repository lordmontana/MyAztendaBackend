using ProductService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Product { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // You can override OnModelCreating if you want to customize your database schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

}

