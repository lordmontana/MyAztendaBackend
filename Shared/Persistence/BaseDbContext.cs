// Shared.Persistence.TenantDbContext<TSelf>
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Shared.Admin.Interfaces;
using Shared.Entities;
using Shared.Logging.Enities;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;

namespace Shared.Persistence;

/// Generic base for every service DbContext
public abstract class BaseDbContext<TSelf> : DbContext
    where TSelf : DbContext
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IUserInfoProvider _user;
    protected abstract string AuditTableName { get; }
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();      // one audit DbSet

    protected BaseDbContext(IUserInfoProvider user,
                            IHttpContextAccessor accessor,
                             DbContextOptions<TSelf> opts)
        : base(opts)
    {
        _user = user;
        _accessor = accessor;
    }

    /*──────────────── GLOBAL tenant filter ────────────────*/
    protected override void OnModelCreating(ModelBuilder b)
    {
        // map the AuditLog entity to the per-service table
        b.Entity<AuditLog>().ToTable(AuditTableName);
        // add tenant filter to every BaseEntity
        foreach (var et in b.Model.GetEntityTypes()
                                  .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
        {
            var param = Expression.Parameter(et.ClrType, "e");
            var body = Expression.Equal(
                            Expression.Property(
                                Expression.Convert(param, typeof(BaseEntity)),
                                nameof(BaseEntity.IId)),
                            Expression.Constant(_user.InstallationId));

            et.SetQueryFilter(Expression.Lambda(body, param));
        }

        base.OnModelCreating(b);
    }

    /*──────────────── SaveChanges override ────────────────*/
    public override int SaveChanges()
    {
        var batch = CaptureSnapshots();      // ① keep original states
        StampTenant();

        var result = base.SaveChanges();     // ② DB generates real keys

        WriteAuditRows(batch);               // ③ use saved state, real PK
        base.SaveChanges();                  // ④ flush audit rows

        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var batch = CaptureSnapshots();
        StampTenant();

        var result = await base.SaveChangesAsync(ct);

        WriteAuditRows(batch);
        await base.SaveChangesAsync(ct);

        return result;
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
    /*──────────────── helpers ─────────────────────────────*/
    private record AuditSnapshot(EntityEntry Entry, EntityState OriginalState);
    private string? RawJson =>
        _accessor.HttpContext?.Items.TryGetValue("RawJson", out var v) == true ? v as string : null;
    private List<AuditSnapshot> CaptureSnapshots() =>
    ChangeTracker.Entries()
                 .Where(e => e.Metadata.ClrType != typeof(AuditLog) &&
                             (e.State == EntityState.Added ||
                              e.State == EntityState.Modified ||
                              e.State == EntityState.Deleted))
                 .Select(e => new AuditSnapshot(e, e.State))   // save original state
                 .ToList();                                    // snapshot – avoids enum version error

    private void WriteAuditRows(IEnumerable<AuditSnapshot> snaps)
    {
        var now = DateTime.UtcNow;
        var postedJson = RawJson;                          // may be null for GETs

        foreach (var snap in snaps)
        {
            var e = snap.Entry;               // same EntityEntry (now Unchanged / Detached)
            var op = snap.OriginalState switch // original CRUD action
            {
                EntityState.Added => "CREATE",
                EntityState.Modified => "UPDATE",
                _ => "DELETE"
            };

            AuditLogs.Add(new AuditLog
            {
                TableName = e.Metadata.GetTableName()!,
                EntityName = e.Metadata.ClrType.Name,
                EntityId = PrimaryKeyInt(e),                 // real key after save
                Operation = op,
                PerformedBy = _user.GetUserInfo().UserName,
                PerformedAtUtc = now,
                ChangesJson = postedJson ?? "", //?? JsonConvert.SerializeObject(Payload(e, snap.OriginalState)),
                IId = _user.InstallationId
            });
        }
    }

    private static int PrimaryKeyInt(EntityEntry e) =>
        (int)(e.Properties.First(p => p.Metadata.IsPrimaryKey()).CurrentValue ?? 0);

    private static object Payload(EntityEntry e, EntityState original)
    {
        return original == EntityState.Modified
               ? new { Old = e.OriginalValues.ToObject(), New = e.CurrentValues.ToObject() }
               : e.CurrentValues.ToObject();
    }
}