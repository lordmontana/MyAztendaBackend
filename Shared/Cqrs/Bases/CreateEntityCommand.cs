// Shared/Cqrs/Bases/CreateEntityCommand.cs
using Shared.Cqrs.Abstractions;

public abstract record CreateEntityCommand<TEntity, TInput>(TInput Input) : ICommand<int> where TEntity : class;