// Shared/Web/Middleware/GlobalExceptionMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Shared.Web.Exceptions;

namespace Shared.Web.Middleware;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> log)
{
    public async Task Invoke(HttpContext ctx)
    {
        try { await next(ctx); }
        catch (Exception ex) { await HandleAsync(ctx, ex, log); }
    }

    private static Task HandleAsync(HttpContext ctx, Exception ex, ILogger log)
    {
        var result = ex switch
        {
            ValidationErrorException v => ((int)v.StatusCode,
                (object)new { message = v.Message, errors = v.Errors }),

            DomainRuleException d => (d.StatusCode,
                (object)new { message = d.Message }),

            _ => (500,
                (object)new { message = "Unexpected error" })
        }; 

        var (status, body) = result;

        // Log only when it’s an unexpected (500) case
        if (status == 500)
            log.LogError(ex, "Unhandled exception"); 

        var json = JsonSerializer.Serialize(body);
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        return ctx.Response.WriteAsync(json);
    }
}
