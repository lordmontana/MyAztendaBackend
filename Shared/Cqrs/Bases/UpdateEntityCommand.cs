// UpdateEntityCommand.cs
using Shared.Cqrs.Abstractions;
public abstract record UpdateEntityCommand<TEntity, TInput>(int Id, TInput Input)
            : ICommand where TEntity : class;
