using Microsoft.AspNetCore.Mvc;
using EmployeeService.Services;
using EmployeeLocationService;

namespace EmployeeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationGRPCTestController : ControllerBase
    {
        private readonly EmployeeGRPCClientService _grpcClientService;

        public LocationGRPCTestController(EmployeeGRPCClientService grpcClientService)
        {
            _grpcClientService = grpcClientService;
        }

        // Fetch all locations
        [HttpGet("GetAllLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            var result = await _grpcClientService.GetAllLocationsAsync();
            return Ok(result.Locations); // Return the list of locations
        }

        // Fetch a specific location by ID
        [HttpGet("GetLocationById/{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _grpcClientService.GetLocationByIdAsync(id);
            return location is null ? NotFound($"Location with ID {id} not found.") : Ok(location);
        }
    }
}
