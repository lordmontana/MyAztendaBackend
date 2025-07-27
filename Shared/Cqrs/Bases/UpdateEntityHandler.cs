using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;
using Shared.Web.Exceptions;

namespace Shared.Cqrs.Bases;

/// <summary>
/// Template-method base for UPDATE commands.
/// TCommand must expose an <c>EmployeeDto Employee</c> property and (optionally) an <c>int Id</c>.
/// </summary>
public abstract class UpdateEntityHandler<TEntity, TCommand> :
    ICommandHandler<TCommand, int>          // returns Id that was updated
    where TEntity : class
    where TCommand : ICommand<int>
{
    protected readonly IRepository<TEntity> _repo;
    protected readonly DbContext _db;

    protected UpdateEntityHandler(IRepository<TEntity> repo, DbContext db)
    {
        _repo = repo;
        _db = db;
    }

    public async Task<int> HandleAsync(TCommand cmd, CancellationToken ct)
    {
        // 1) Fetch existing
        var entity = await GetExistingAsync(cmd, ct)
            ?? throw new DomainRuleException($"Entity not found.");

        // 2) Pre-update domain rules
        await CheckBusinessRulesAsync(cmd, entity, ct);

        // 3) Map incoming values onto entity
        Map(cmd, entity);

        _repo.Update(entity);
        await _repo.SaveChangesAsync();

        // 4) Post-save hook
        await AfterSaveAsync(entity, ct);
        return GetId(entity);
    }

    protected abstract Task<TEntity?> GetExistingAsync(TCommand cmd, CancellationToken ct);
    protected abstract int GetId(TEntity entity);
    protected virtual Task CheckBusinessRulesAsync(TCommand cmd, TEntity entity, CancellationToken ct)
        => Task.CompletedTask;

    protected abstract void Map(TCommand cmd, TEntity entity);

    protected virtual Task AfterSaveAsync(TEntity entity, CancellationToken ct)
        => Task.CompletedTask;
}