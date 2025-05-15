using CustomerService.DTOs;
using CustomerService.Models;
using CustomerService.Persistence;
using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly ApplicationDbContext _context;

    public CustomersController(ILogger<CustomersController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var repo = new Repository<Customer>(_context);
        var items = await repo.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {

        var repo = new Repository<Customer>(_context);
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerDto dto)
    {

        var repo = new Repository<Customer>(_context);

        var customer = new Customer
        {
            Name = dto.Name,
            Gender = dto.Gender,
        };

        await repo.AddAsync(customer);
        await repo.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomerDto dto)
    {

        var repo = new Repository<Customer>(_context);

        var customer = await repo.GetByIdAsync(id);
        if (customer is null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Name)) customer.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Gender)) customer.Gender = dto.Gender;


        repo.Update(customer);
        await repo.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var repo = new Repository<Customer>(_context);

        var customer = await repo.GetByIdAsync(id);
        if (customer is null) return NotFound();

        repo.Delete(customer);
        await repo.SaveChangesAsync();

        return NoContent();
    }
}
