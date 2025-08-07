using LocationService.DTOs;

namespace LocationService.Application.Cqrs.Commands.LocationForm.CRUD
{
    public interface ILocationPayload
    {
        LocationDto Location { get; }
    }
}
