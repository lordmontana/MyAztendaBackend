using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocationService.Data;
using LocationService.Entities;
using LocationService.DTOs;
using Shared.Repositories;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace LocationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILogger<LocationsController> _logger;
    private readonly ApplicationDbContext _context;

    public LocationsController(ILogger<LocationsController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		try
		{
			var repo = new Repository<Location>(_context);
			var items = await repo.GetAllAsync();
			return Ok(items);
		}
		catch (Exception ex)
		{
			// You can log here, too (e.g., to file, Grafana Loki, etc.)
			return StatusCode(500, new
			{
				message = "An error occurred while retrieving data.",
				error = ex.Message,        // Optional: return ex.StackTrace for full trace
				type = ex.GetType().Name
			});
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		
		var repo = new Repository<Location>(_context);
		var item = await repo.GetByIdAsync(id);
		return item is null ? NotFound() : Ok(item);
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateLocationDto dto)
	{
		
		var repo = new Repository<Location>(_context);

		var location = new Location
		{
			Name = dto.Name,
			Region = dto.Region,
			ClientId = dto.ClientId
		};

		await repo.AddAsync(location);
		await repo.SaveChangesAsync();

		return CreatedAtAction(nameof(GetById), new { id = location.Id }, location);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, UpdateLocationDto dto)
	{
		
		var repo = new Repository<Location>(_context);

		var location = await repo.GetByIdAsync(id);
		if (location is null) return NotFound();

		if (!string.IsNullOrWhiteSpace(dto.Name)) location.Name = dto.Name;
		if (!string.IsNullOrWhiteSpace(dto.Region)) location.Region = dto.Region;
		if (dto.ClientId.HasValue) location.ClientId = dto.ClientId.Value;

		repo.Update(location);
		await repo.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		
		var repo = new Repository<Location>(_context);

		var location = await repo.GetByIdAsync(id);
		if (location is null) return NotFound();

		repo.Delete(location);
		await repo.SaveChangesAsync();

		return NoContent();
	}



}
