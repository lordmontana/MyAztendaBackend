// DeleteEntityCommand.cs
using Shared.Cqrs.Abstractions;
public abstract record DeleteEntityCommand<TEntity>(int Id) : ICommand
            where TEntity : class;
