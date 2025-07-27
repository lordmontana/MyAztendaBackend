// Shared/Cqrs/Bases/SaveEntityHandler.cs
using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;
using Shared.Web.Exceptions;

namespace Shared.Cqrs.Bases;

public abstract class SaveEntityHandler<TEntity, TCommand> :
    ICommandHandler<TCommand, int>
    where TEntity : class
    where TCommand : ICommand<int>
{
    protected enum ActionKind { Create, Update }

    protected IRepository<TEntity> _repo;

    protected SaveEntityHandler(IRepository<TEntity> repo)
    {
        _repo = repo;
    }

    public async Task<int> HandleAsync(TCommand cmd, CancellationToken ct)
    {
        // ① Determine action and load (or create) entity
        (ActionKind action, TEntity entity) = await PrepareEntityAsync(cmd, ct);

        // ② Pre-save business rules
        await BeforeSaveAsync(cmd, entity, action, ct);

        // ③ Map incoming data
        Map(cmd, entity, action);

        if (action == ActionKind.Create)
            await _repo.AddAsync(entity);

        await _repo.SaveChangesAsync();     // works for both create & update

        // ④ Post-save hook
        await AfterSaveAsync(entity, action, ct);

        return GetId(entity);
    }

    // ─────────── hooks the concrete class must/ may override ────────────
    protected abstract Task<(ActionKind Action, TEntity Entity)>
        PrepareEntityAsync(TCommand cmd, CancellationToken ct);

    protected abstract int GetId(TEntity entity);

    protected virtual Task BeforeSaveAsync(
        TCommand cmd, TEntity entity, ActionKind action, CancellationToken ct) => Task.CompletedTask;

    protected abstract void Map(
        TCommand cmd, TEntity entity, ActionKind action);

    protected virtual Task AfterSaveAsync(
        TEntity entity, ActionKind action, CancellationToken ct) => Task.CompletedTask;
}
