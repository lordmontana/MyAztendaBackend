using Shared.Logging.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Logging.Interfaces
{
    public interface IAuditLogger
    {
        Task LogAsync(AuditLog entry, CancellationToken ct = default);

    }
}
