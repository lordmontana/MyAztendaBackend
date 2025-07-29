using Microsoft.Extensions.DependencyInjection;
using Shared.Logging.Interfaces;

namespace Shared.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the concrete audit logger supplied by the service
    /// plus any shared options like JSON serialization.
    /// </summary>
    public static IServiceCollection AddAuditLogging<TLogger>(this IServiceCollection services) where TLogger : class, IAuditLogger
    {
        return services.AddScoped<IAuditLogger, TLogger>();
    }
}
