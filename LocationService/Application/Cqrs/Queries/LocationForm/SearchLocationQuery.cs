
using EmployeeLocationService;
using LocationService.DTOs;
using Shared.Cqrs.Bases;
using Shared.Dtos;

namespace LocationService.Application.Cqrs.Queries.LocationForm;
public sealed record SearchLocationQuery(
    int Page, int PageSize, string Mode, List<FilterDto> Filters)
    : PagedSearchQuery<Entities.Forms.Location, LocationDto>(Page, PageSize, Mode, Filters);

