using EmployeeService.DTOs;
using EmployeeService.Entities.Forms;
using EmployeeService.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Admin.Interfaces;
using Shared.Dtos;
using Shared.Filtering;
using Shared.Repositories;
using Shared.Repositories.Abstractions;
using Shared.Repositories.Persistence;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace EmployeeService.Controllers;

[Authorize] 
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IUserInfoProvider _Admin;   // 
    private readonly IRepository<Employee> _repo;


    public EmployeesController(ILogger<EmployeesController> logger, ApplicationDbContext context,IUserInfoProvider Admin, IRepository<Employee> repo)
    {
        _logger = logger;
        _context = context;
        _Admin = Admin;  
        _repo = repo;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var repo = new Repository<Employee>(_context);
			var items = await repo.GetAllAsync();
			return Ok(items);
		}
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");

			// You can log here, too (e.g., to file, Grafana Loki, etc.)
			return StatusCode(500, new
            {
                message = "An error occurred while retrieving data.",
				error = ex.Message,        // Optional: return ex.StackTrace for full trace
				type = ex.GetType().Name
			});
		}
     
    }

    [HttpPost("search")]                        // POST /api/employees/search
    public async Task<IActionResult> Search([FromBody] PagedRequest req)
    {
        try
        {
            var parser = ParserFactory.Get<Employee>(req.Mode);

            var predicates = parser.Parse(req.Filters ?? new());

            var result = await _repo.QueryAsync(
                page: Math.Max(req.Page, 0),
                pageSize: Math.Clamp(req.PageSize, 1, 100),
                orderBy: e => e.Name,
                ascending: true,
                filters: predicates);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Get employees failed");
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {

        var repo = new Repository<Employee>(_context);
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeDto dto)
    {

        var repo = new Repository<Employee>(_context);

        var employee = new Employee
        {
            Name = dto.Name,
            Gender = dto.Gender,
        };

        await repo.AddAsync(employee);
        await repo.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EmployeeDto dto)
    {

        var repo = new Repository<Employee>(_context);

        var employee = await repo.GetByIdAsync(id);
        if (employee is null) return NotFound(); 

        if (!string.IsNullOrWhiteSpace(dto.Name)) employee.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Gender)) employee.Gender = dto.Gender;


        repo.Update(employee);
        await repo.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var repo = new Repository<Employee>(_context);

        var employee = await repo.GetByIdAsync(id);
        if (employee is null) return NotFound();

        repo.Delete(employee);
        await repo.SaveChangesAsync();

        return NoContent();
    }
}
