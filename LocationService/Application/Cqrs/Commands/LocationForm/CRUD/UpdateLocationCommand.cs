using LocationService.DTOs;
using Shared.Cqrs.Abstractions;

namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD;
public sealed record UpdateLocationCommand(int id,LocationDto Location) : ICommand<int>, ILocationPayload;


