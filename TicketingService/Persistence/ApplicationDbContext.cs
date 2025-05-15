using TicketingService.Entities;
using Microsoft.EntityFrameworkCore;

namespace TicketingService.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Ticket> Ticket { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // You can override OnModelCreating if you want to customize your database schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

}

