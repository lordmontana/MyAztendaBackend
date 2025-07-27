// DeleteEntityHandler.cs
using Shared.Cqrs.Abstractions;
using Shared.Repositories.Abstractions;

namespace Shared.Cqrs.Bases;

public abstract class DeleteEntityHandler<TEntity, TCommand>
        : ICommandHandler<TCommand>
        where TEntity : class
        where TCommand : ICommand
{
    private readonly IRepository<TEntity> _repo;
    protected DeleteEntityHandler(IRepository<TEntity> repo) => _repo = repo;

    protected abstract int GetId(TCommand cmd);

    public async Task HandleAsync(TCommand cmd, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(GetId(cmd))
                     ?? throw new InvalidOperationException("Not found");
        _repo.Delete(entity);
        await _repo.SaveChangesAsync();
    }
}
