using Microsoft.EntityFrameworkCore;
using Shared.Logging.Enities;
using Shared.Logging.Interfaces;

namespace Shared.Logging.Base;

/// <summary>
/// Generic logger that writes to a service-specific DbContext and table.
/// The concrete service supplies the DbContext via DI.
/// </summary>
public abstract class BaseAuditLogger<TContext> : IAuditLogger
    where TContext : DbContext
{
    private readonly TContext _db;
    private readonly string _tableName;

    protected BaseAuditLogger(TContext db, string tableName)
    {
        _db = db;
        _tableName = tableName;
    }

    public async Task LogAsync(AuditLog entry, CancellationToken ct = default)
    {
        // EF Core can map to a table created separately in each service.
        _db.Set<AuditLog>().Add(entry);
        await _db.SaveChangesAsync(ct);
    }
}
