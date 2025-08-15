using LocationService.Application.Cqrs.Commands.LocationForm.CRUD;
using LocationService.Application.Cqrs.Queries.LocationForm;
using LocationService.DTOs;
using LocationService.Entities.Forms;
using LocationService.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Admin.Interfaces;
using Shared.Cqrs;
using Shared.Dtos;
using Shared.Repositories;
using Shared.Repositories.Abstractions;
using Shared.Repositories.Persistence;

namespace LocationService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILogger<LocationsController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IUserInfoProvider _Admin;   
    private readonly MiniMediator _med;

    public LocationsController(ILogger<LocationsController> logger,  ApplicationDbContext context, IUserInfoProvider Admin, MiniMediator med)
    {
        _logger = logger;
        _context = context;
		_Admin = Admin;
		_med = med;
    }

    /// <summary>Search employees with paging & dynamic filters.</summary>
    [HttpPost("search")]
    public Task<PagedResult<LocationDto>> SearchEmployees([FromBody] PagedRequest req)
        => _med.SendAsync(new SearchLocationQuery(
               req.Page, req.PageSize, req.Mode, req.Filters ?? new()));

    [HttpPost]
    public Task<int> Create([FromBody] LocationDto dto) => _med.SendAsync(new CreateLocationCommand(dto));


    [HttpPut("{id:int}")]
    public Task<int> Update(int id, [FromBody] LocationDto dto) =>
    _med.SendAsync(new UpdateLocationCommand(id, dto));


    [HttpDelete("{id:int}")]
    public Task Delete(int id)
    => _med.SendAsync(new DeleteLocationCommand(id));

}
