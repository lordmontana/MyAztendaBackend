using EmployeeLocationService;
using Grpc.Core;
using LocationService.Data;
using LocationService.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Services
{
    public class EmployeeLocationServiceImp : EmployeeLocationGRPC.EmployeeLocationGRPCBase
    {
        private readonly ILogger<EmployeeLocationServiceImp> _logger;
        private readonly ApplicationDbContext _context;

        public EmployeeLocationServiceImp(ILogger<EmployeeLocationServiceImp> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Fetch all locations
        public override async Task<LocationList> GetLocations(EmptyRequest request, ServerCallContext context)
        {
            var locations = await _context.Location.ToListAsync();

            var locationList = new LocationList();
            locationList.Locations.AddRange(locations.Select(l => new EmployeeLocationService.Location
            {
                Id = l.Id,
                Name = l.Name,
                Region = l.Region,
                ClientId = l.ClientId
            }));

            return locationList;
        }

        // Fetch a specific location by ID
        public override async Task<EmployeeLocationService.Location> GetLocationById(LocationRequest request, ServerCallContext context)
        {
            var location = await _context.Location.FindAsync(request.Id);
            if (location == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Location with ID {request.Id} not found."));
            }

            return new EmployeeLocationService.Location
            {
                Id = location.Id,
                Name = location.Name,
                Region = location.Region,
                ClientId = location.ClientId
            };
        }
    }
}
