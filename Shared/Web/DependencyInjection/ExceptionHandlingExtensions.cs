// Shared/Web/DependencyInjection/ExceptionHandlingExtensions.cs
using Microsoft.AspNetCore.Builder;
using Shared.Web.Middleware;

namespace Shared.Web.DependencyInjection;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionMiddleware>();
}
