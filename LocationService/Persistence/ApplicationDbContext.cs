using Microsoft.EntityFrameworkCore;
using LocationService.Entities.Forms;

namespace LocationService.Persistence;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public DbSet<Location> Location { get; set; }

    // You can override OnModelCreating if you want to customize your database schema
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasKey(p => p.Id);
        base.OnModelCreating(modelBuilder);
    }
}




