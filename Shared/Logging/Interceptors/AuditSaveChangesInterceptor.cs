using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Shared.Admin.Interfaces;      // IUserInfoProvider  (or your Admin object)
using Shared.Logging.Enities;
using Shared.Logging.Interfaces;    // IAuditLogger

namespace Shared.Logging.Interceptors;

/// Intercepts SaveChanges / SaveChangesAsync and logs every INSERT / UPDATE / DELETE.
/// No guessing: table name, PK and old/new values come straight from EF metadata.
public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IAuditLogger _audit;
    private readonly IUserInfoProvider _user;   // holds Username, InstallationId, etc.

    public AuditSaveChangesInterceptor(IAuditLogger audit, IUserInfoProvider user)
    {
        _audit = audit;
        _user = user;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        var db = eventData.Context!;
        var now = DateTime.UtcNow;

        foreach (var entry in db.ChangeTracker.Entries()
                                 .Where(e => e.State is EntityState.Added
                                               or EntityState.Modified
                                               or EntityState.Deleted))
        {
            await _audit.LogAsync(ToAuditLog(entry, now), ct);
        }

        return await base.SavingChangesAsync(eventData, result, ct);
    }

    /*──────────────────────── helpers ────────────────────────*/

    private AuditLog ToAuditLog(EntityEntry e, DateTime now) => new()
    {
        TableName = e.Metadata.GetTableName()!,       // exact table
        EntityName = e.Metadata.ClrType.Name,
        EntityId = e.PrimaryKeyInt(),
        Operation = e.State switch
        {
            EntityState.Added => "CREATE",
            EntityState.Modified => "UPDATE",
            _ => "DELETE"
        },
        PerformedBy = _user.GetUserInfo().UserName,
        ChangesJson = JsonConvert.SerializeObject(e.Payload()),
        IId = _user.GetUserInfo().InstallationId
    };
}

internal static class AuditEntryExtensions
{
    public static int PrimaryKeyInt(this EntityEntry e) =>
        (int)(e.Properties.First(p => p.Metadata.IsPrimaryKey()).CurrentValue ?? 0);

    public static object Payload(this EntityEntry e) =>
        e.State == EntityState.Modified
        ? new { Old = e.OriginalValues.ToObject(), New = e.CurrentValues.ToObject() }
        : e.CurrentValues.ToObject();
}
