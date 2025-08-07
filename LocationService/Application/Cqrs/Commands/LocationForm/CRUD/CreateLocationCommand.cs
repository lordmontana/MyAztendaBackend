using LocationService.DTOs;
using Shared.Cqrs.Abstractions;

namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD
{
 
    public sealed record CreateLocationCommand(LocationDto Location)
        : ICommand<int>, ILocationPayload; // returns new Id
}
