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
    /// <summary>One simple field/value filter coming from the UI.</summary>
    public sealed record FilterDto(string Field, string Value);
    /// <summary>Request wrapper used by list & popup endpoints.</summary>
    public sealed record PagedRequest(
        int Page = 0,
        int PageSize = 25,
        string Mode = "simple",          // "simple" or "advanced"
        List<FilterDto>? Filters = null);
}
