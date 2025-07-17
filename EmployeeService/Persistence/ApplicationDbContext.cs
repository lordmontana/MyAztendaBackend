using Microsoft.EntityFrameworkCore;
using Shared.Admin.Interfaces;     // ← IUserInfoProvider
using System.Linq.Expressions;
using Shared.Entities;
using EmployeeService.Entities;           // for IHasInstallation

namespace EmployeeService.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IUserInfoProvider _user;

        public ApplicationDbContext(
            IUserInfoProvider user,
            DbContextOptions<ApplicationDbContext> options)
            : base(options) => _user = user;

        // DbSets
        public DbSet<Employee> Employee => Set<Employee>();
        public DbSet<TimeTable> TimeTableForms => Set<TimeTable>();


        // ────────────────────────────────────────────────────
        // Global tenant filter (read isolation)
        // ────────────────────────────────────────────────────
        protected override void OnModelCreating(ModelBuilder b)
        {
            // GLOBAL filter applies to every entity that derives from BaseEntity
            foreach (var et in b.Model.GetEntityTypes()
                                      .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
            {
                var p = Expression.Parameter(et.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Property(
                        Expression.Convert(p, typeof(BaseEntity)),
                        nameof(BaseEntity.IId)),
                    Expression.Constant(_user.InstallationId));

                et.SetQueryFilter(Expression.Lambda(body, p));
            }

            base.OnModelCreating(b);
        }

        // ────────────────────────────────────────────────────
        // Auto-stamp InstallationId on inserts (write isolation)
        // ────────────────────────────────────────────────────
        public override int SaveChanges() => StampAndSave();
        public override Task<int> SaveChangesAsync(
            CancellationToken ct = default) => StampAndSaveAsync(ct);

        private int StampAndSave()
        {
            StampTenant();
            return base.SaveChanges();
        }
        private Task<int> StampAndSaveAsync(CancellationToken ct)
        {
            StampTenant();
            return base.SaveChangesAsync(ct);
        }
        private void StampTenant()
        {
            var now = DateTime.UtcNow;

            foreach (var e in ChangeTracker.Entries<BaseEntity>())
                {
                e.Entity.IId = _user.InstallationId;
                if (e.State == EntityState.Added)
                {
                    e.Entity.IId = _user.InstallationId;
                    e.Entity.CreatedUtc = now;
                }
                if (e.State == EntityState.Modified)
                {
                    e.Entity.UpdatedUtc = now;
                }
            }
        }
    }
}
