using Microsoft.Extensions.Configuration;

namespace Shared.Database;

public static class ConnectionStringResolver
{
	public static string GetDefault(IConfiguration config)
	{
		var connectionString = config.GetConnectionString("DefaultConnection");

		if (string.IsNullOrWhiteSpace(connectionString))
			throw new InvalidOperationException("DefaultConnection is not set in the configuration.");

		return connectionString;
	}
}
