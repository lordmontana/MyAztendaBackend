using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    /// <summary>
    /// Generic wrapper for paged queries.
    /// </summary>
    public sealed record PagedResult<T>(IEnumerable<T> Data, int Total);
    
}
