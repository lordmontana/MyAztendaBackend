//using LocationService.GrpcClients;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/[controller]")]
//public class LocationsController : ControllerBase
//{
//    private readonly EmployeeGrpcClient _employeeGrpcClient;

//    public LocationsController(EmployeeGrpcClient employeeGrpcClient)
//    {
//        _employeeGrpcClient = employeeGrpcClient;
//    }

//    [HttpGet("employees")]
//    public async Task<IActionResult> GetAllEmployees()
//    {
//        var employees = await _employeeGrpcClient.GetAllEmployeesAsync();
//        return Ok(employees);
//    }

//    [HttpGet("employees/{id}")]
//    public async Task<IActionResult> GetEmployeeById(int id)
//    {
//        var employee = await _employeeGrpcClient.GetEmployeeByIdAsync(id);
//        return employee is null ? NotFound() : Ok(employee);
//    }
//}
