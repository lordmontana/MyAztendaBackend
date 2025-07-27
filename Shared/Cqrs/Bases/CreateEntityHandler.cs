// CreateEntityHandler.cs
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;

namespace Shared.Cqrs.Bases;

/// <summary>Generic handler for create commands.</summary>
public abstract class CreateEntityHandler<TEntity, TCommand>
        : ICommandHandler<TCommand, int>
        where TEntity : class
        where TCommand : ICommand<int>
{

    protected readonly IRepository<TEntity> _repo;
    protected CreateEntityHandler(IRepository<TEntity> repo) => _repo = repo;


    public async Task<int> HandleAsync(TCommand cmd, CancellationToken ct)
    {
        await CheckBusinessRulesAsync(cmd, ct);       // ← SaveRecordBefore

        var entity = Map(cmd);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();

        await AfterSaveAsync(entity, ct);             // ← SaveRecordAfter

        return (int)typeof(TEntity).GetProperty("Id")!.GetValue(entity)!;
    }
    protected virtual Task CheckBusinessRulesAsync(TCommand cmd, CancellationToken ct) => Task.CompletedTask;
    protected abstract TEntity Map(TCommand cmd);
    protected virtual Task AfterSaveAsync(TEntity entity, CancellationToken ct) => Task.CompletedTask;
}
