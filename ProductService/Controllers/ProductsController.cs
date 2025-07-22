using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Entities;
using ProductService.Persistence;
using Shared.Repositories;
using Shared.Repositories.Persistence;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ApplicationDbContext _context;

    public ProductsController(ILogger<ProductsController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var repo = new Repository<Product>(_context);
        var items = await repo.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {

        var repo = new Repository<Product>(_context);
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto dto)
    {

        var repo = new Repository<Product>(_context);

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock
        };

        await repo.AddAsync(product);
        await repo.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDto dto)
    {

        var repo = new Repository<Product>(_context);

        var product = await repo.GetByIdAsync(id);
        if (product is null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Name)) product.Name = dto.Name;
        product.Price = dto.Price;
        product.Stock = dto.Stock;


        repo.Update(product);
        await repo.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var repo = new Repository<Product>(_context);

        var product = await repo.GetByIdAsync(id);
        if (product is null) return NotFound();

        repo.Delete(product);
        await repo.SaveChangesAsync();

        return NoContent();
    }
}
