using FluentValidation;
using FluentValidation.Results;
using Shared.Cqrs.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Shared.Cqrs.Decorators;

public sealed class ValidationCommandDecorator<TCommand>(
    ICommandHandler<TCommand> inner,
    IEnumerable<IValidator<TCommand>> validators)
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public async Task HandleAsync(TCommand cmd, CancellationToken ct)
    {
        await EnsureValidAsync(cmd, ct);
        await inner.HandleAsync(cmd, ct);
    }

    private async Task EnsureValidAsync(TCommand cmd, CancellationToken ct)
    {
        var context = new ValidationContext<TCommand>(cmd);
        FluentValidation.Results.ValidationResult[] results = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, ct)));

        var failures = results.SelectMany(r => r.Errors).ToArray();
        if (failures.Length > 0)
            throw new FluentValidation.ValidationException(failures);
    }
}
