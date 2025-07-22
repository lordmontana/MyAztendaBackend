using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs.Abstractions;

namespace Shared.Cqrs;

public sealed class MiniMediator
{
    private readonly IServiceProvider _sp;
    public MiniMediator(IServiceProvider sp) => _sp = sp;

    public Task SendAsync(ICommand cmd, CancellationToken ct = default) =>
        Resolve(typeof(ICommandHandler<>), cmd.GetType())
            .HandleAsync((dynamic)cmd, ct);

    public Task<T> SendAsync<T>(ICommand<T> cmd, CancellationToken ct = default) =>
        Resolve(typeof(ICommandHandler<,>), cmd.GetType(), typeof(T))
            .HandleAsync((dynamic)cmd, ct);

    public Task<T> SendAsync<T>(IQuery<T> qry, CancellationToken ct = default) =>
        Resolve(typeof(IQueryHandler<,>), qry.GetType(), typeof(T))
            .HandleAsync((dynamic)qry, ct);

    private dynamic Resolve(Type openGeneric, params Type[] typeArgs) =>
        _sp.GetRequiredService(openGeneric.MakeGenericType(typeArgs));
}
