using EmployeeService.Entities.Forms;           // for IHasInstallation
using Microsoft.EntityFrameworkCore;
using Shared.Admin.Interfaces;     // ← IUserInfoProvider
using Shared.Entities;
using Shared.Logging.Enities;
using Shared.Logging.Interceptors;
using Shared.Persistence;
using System.Linq.Expressions;

namespace EmployeeService.Persistence
{
    public class ApplicationDbContext :BaseDbContext<ApplicationDbContext>
    {

        public ApplicationDbContext(
            IUserInfoProvider user,
            IHttpContextAccessor accessor,   // ← add this
            DbContextOptions<ApplicationDbContext> options)
            : base(user, accessor, options) {}

        // DbSets
        public DbSet<Employee> Employee => Set<Employee>();
        public DbSet<TimeTable> TimeTableForms => Set<TimeTable>();
        public DbSet<AuditLog> HumanResourcesAuditLog => Set<AuditLog>();
        protected override string AuditTableName => "HumanResourcesAuditLog";

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.HasDefaultSchema("atzenda");   // for postgres
            base.OnModelCreating(b);

            // map audit table name once
            b.Entity<AuditLog>().ToTable("HumanResourcesAuditLog");
        }
      
    }
}
