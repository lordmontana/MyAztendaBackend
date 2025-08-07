using Shared.Cqrs.Abstractions;

namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD;

    public sealed record DeleteLocationCommand(int Id) : ICommand;

    

