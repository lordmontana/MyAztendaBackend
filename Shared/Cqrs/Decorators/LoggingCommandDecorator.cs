using Microsoft.Extensions.Logging;
using Shared.Cqrs.Abstractions;

namespace Shared.Cqrs.Decorators;

public sealed class LoggingCommandDecorator<TCommand>(
    ICommandHandler<TCommand> inner,
    ILogger<LoggingCommandDecorator<TCommand>> log)
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public async Task HandleAsync(TCommand cmd, CancellationToken ct)
    {
        log.LogInformation("Handling {Command}", typeof(TCommand).Name);
        await inner.HandleAsync(cmd, ct);
        log.LogInformation("Handled  {Command}", typeof(TCommand).Name);
    }
}
