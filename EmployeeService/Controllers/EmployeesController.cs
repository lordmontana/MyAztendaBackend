using EmployeeService.DTOs;
using EmployeeService.Entities.Forms;
using EmployeeService.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Admin.Interfaces;
using Shared.Dtos;
using Shared.Repositories.Abstractions;
using Shared.Repositories.Persistence;
using System.Security.Claims;
using Shared.Cqrs;          

using EmployeeService.Application.Cqrs.Commands.EmployeeForm.CRUD;
using EmployeeService.Application.Cqrs.Queries.EmployeeForm;



namespace EmployeeService.Controllers;

[Authorize] 
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
  //  private readonly ApplicationDbContext _context;
   // private readonly IUserInfoProvider _Admin;   // 
    private readonly MiniMediator _med;


    public EmployeesController(ILogger<EmployeesController> logger,MiniMediator med)
    {
        _logger = logger;
        _med = med;
    }

   //public EmployeesController(ILogger<EmployeesController> logger, ApplicationDbContext context, IUserInfoProvider Admin, MiniMediator med)
   //{
   //    _logger = logger;
   //    _context = context;
   //    _Admin = Admin;
   //    _med = med;
   //}


    // [HttpGet]//SearchEmployees does the same. we will never give this endpoint to frontend. Only with pagination etc
    // public async Task<IActionResult> GetAll()
    // {
    //  
    //		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //		var repo = new Repository<Employee>(_context);
    //		var items = await repo.GetAllAsync();
    //		return Ok(items);
    //
    // }

    /// <summary>Search employees with paging & dynamic filters.</summary>
    [HttpPost("search")]
    public Task<PagedResult<EmployeeDto>> SearchEmployees([FromBody] PagedRequest req)
        => _med.SendAsync(new SearchEmployeesQuery(
               req.Page, req.PageSize, req.Mode, req.Filters ?? new()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _med.SendAsync(new GetEmployeeByIdQuery(id));
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]                                      
    public Task<int> Create([FromBody] EmployeeDto dto) => _med.SendAsync(new CreateEmployeeCommand(dto));

    [HttpPut("{id:int}")]
    public Task<int> Update(int id, [FromBody] EmployeeDto dto) =>
    _med.SendAsync(new UpdateEmployeeCommand(id, dto));

    [HttpDelete("{id:int}")]
    public Task Delete(int id)
    => _med.SendAsync(new DeleteEmployeeCommand(id));

}
