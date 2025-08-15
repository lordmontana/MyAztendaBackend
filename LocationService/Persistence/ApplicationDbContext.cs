using LocationService.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Shared.Admin.Interfaces;
using Shared.Persistence;

namespace LocationService.Persistence;

public class ApplicationDbContext : BaseDbContext<ApplicationDbContext>
{
    public ApplicationDbContext(
             IUserInfoProvider user,
             IHttpContextAccessor accessor,   // ← add this
             DbContextOptions<ApplicationDbContext> options)
             : base(user, accessor, options) { }

    public DbSet<Location> Location { get; set; }

    protected override string AuditTableName => "LocationAuditLog";

    // You can override OnModelCreating if you want to customize your database schema
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("atzenda");   // for postgres

        modelBuilder.Entity<Location>().HasKey(p => p.Id);
        base.OnModelCreating(modelBuilder);
    }
}




