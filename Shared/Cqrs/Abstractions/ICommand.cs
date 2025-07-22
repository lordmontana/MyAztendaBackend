
namespace Shared.Cqrs.Abstractions;

public interface ICommand { }
public interface ICommand<TResponse> : ICommand { }