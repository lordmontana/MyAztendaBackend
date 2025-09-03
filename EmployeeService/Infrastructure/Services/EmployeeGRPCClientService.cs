using EmployeeLocationService;
using Grpc.Net.Client;
using static EmployeeLocationService.EmployeeLocationGRPC;

namespace EmployeeService.Infrastructure.Services
{
    public class EmployeeGRPCClientService
    {
        private readonly EmployeeLocationGRPC.EmployeeLocationGRPCClient _client;

        public EmployeeGRPCClientService(string grpcServerUrl)
        {
            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(grpcServerUrl);
            _client = new EmployeeLocationGRPC.EmployeeLocationGRPCClient(channel);
        }


        // Fetch all locations
        public async Task<LocationList> GetAllLocationsAsync()
        {
            var request = new EmptyRequest(); // Empty request for fetching all locations
            var response = await _client.GetLocationsAsync(request);
            return response;
        }

        // Fetch a specific location by ID
        public async Task<Location?> GetLocationByIdAsync(int id)
        {
            try
            {
                var request = new LocationRequest { Id = id }; // Request with location ID
                var response = await _client.GetLocationByIdAsync(request);
                return response;
            }
            catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return null; // Return null if the location is not found
            }
        }
    }
}
