using EmployeeLocationService;
using LocationService.DTOs;
using Shared.Cqrs.Bases;
using Shared.Repositories.Abstractions;
using System.Linq.Expressions;

namespace LocationService.Application.Cqrs.Queries.LocationForm;


public sealed class SearchLocationHandler(
        IRepository<Entities.Forms.Location> repo)
    : PagedSearchHandler<Entities.Forms.Location, LocationDto, SearchLocationQuery>(repo)
{
    protected override Func<Entities.Forms.Location, LocationDto> Map =>
     e => new LocationDto(
            id: e.Id,
            createdUtc: e.CreatedUtc,        // DateTime
            updatedUtc: e.UpdatedUtc,        // DateTime?
            name: e.Name,
            description: e.Description,
            type: e.Type,
            cropType: e.CropType,
            cropTypeId: e.CropTypeId,
            addressLine1: e.AddressLine1,
            addressLine2: e.AddressLine2,
            city: e.City,
            state: e.State,
            country: e.Country,
            postcode: e.Postcode,
            latitude: e.Latitude,         // decimal?
            longitude: e.Longitude,        // decimal?
            elevationMeters: e.ElevationMeters,  // double?
            contactPerson: e.ContactPerson,
            contactPhone: e.ContactPhone,
            contactEmail: e.ContactEmail,
            parentLocationId: e.ParentLocationId,
            isActive: e.IsActive
        );
    protected override Expression<Func<Entities.Forms.Location, object>> OrderBy => e => e.Name;
}