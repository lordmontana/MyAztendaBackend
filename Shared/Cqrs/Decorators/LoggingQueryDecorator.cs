using Microsoft.Extensions.Logging;
using Shared.Cqrs.Abstractions;

namespace Shared.Cqrs.Decorators;

public sealed class LoggingQueryDecorator<TQuery, TResp>(
    IQueryHandler<TQuery, TResp> inner,
    ILogger<LoggingQueryDecorator<TQuery, TResp>> log)
    : IQueryHandler<TQuery, TResp>
    where TQuery : IQuery<TResp>
{
    public async Task<TResp> HandleAsync(TQuery qry, CancellationToken ct)
    {
        log.LogInformation("Query {Query}", typeof(TQuery).Name);
        var resp = await inner.HandleAsync(qry, ct);
        log.LogInformation("Query {Query} completed", typeof(TQuery).Name);
        return resp;
    }
}
