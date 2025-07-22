using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;
using Shared.Repositories.Persistence;
using TicketingService.DTOs;
using TicketingService.Entities;
using TicketingService.Persistence;

namespace TicketingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketingController : ControllerBase
{
    private readonly ILogger<TicketingController> _logger;
    private readonly ApplicationDbContext _context;

    public TicketingController(ILogger<TicketingController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var repo = new Repository<Ticket>(_context);
        var items = await repo.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {

        var repo = new Repository<Ticket>(_context);
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketDto dto)
    {

        var repo = new Repository<Ticket>(_context);

        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            AssignedTo = dto.AssignedTo,
            CreatedDate = DateTime.UtcNow // Automatically set the creation date
        };


        await repo.AddAsync(ticket);
        await repo.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TicketDto dto)
    {

        var repo = new Repository<Ticket>(_context);

        var ticket = await repo.GetByIdAsync(id);
        if (ticket is null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Title)) ticket.Title = dto.Title;
        if (!string.IsNullOrWhiteSpace(dto.Description)) ticket.Description = dto.Description;
        if (!string.IsNullOrWhiteSpace(dto.Status)) ticket.Status = dto.Status;

        ticket.AssignedTo = dto.AssignedTo; // Update AssignedTo field
        ticket.ClosedDate = dto.Status == "Closed" ? DateTime.UtcNow : null; // Set ClosedDate if status is "Closed"



        repo.Update(ticket);
        await repo.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var repo = new Repository<Ticket>(_context);

        var ticket = await repo.GetByIdAsync(id);
        if (ticket is null) return NotFound();

        repo.Delete(ticket);
        await repo.SaveChangesAsync();

        return NoContent();
    }
}
