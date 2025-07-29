// Shared.Web.Middlewares/RawBodyMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public sealed class RawBodyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RawBodyMiddleware> _log;

    public RawBodyMiddleware(RequestDelegate next, ILogger<RawBodyMiddleware> log)
    {
        _next = next; _log = log;
    }

    //very useful for debugging purposes and later on to store changes  have been done to an entity
    public async Task InvokeAsync(HttpContext ctx)
    {
        // We only care about requests that CAN carry a body
        if (ctx.Request.Method is "POST" or "PUT" or "PATCH" or "DELETE")
        {
            // If ContentLength == 0, there’s definitely no body — skip.
            if (ctx.Request.ContentLength is > 0 || ctx.Request.Body.CanSeek)
            {
                ctx.Request.EnableBuffering();          // allow a second read
                using var sr = new StreamReader(
                       ctx.Request.Body,
                       leaveOpen: true);                 // don't close the stream

                var body = await sr.ReadToEndAsync();
                ctx.Items["RawJson"] = body;            // stash it (may be empty)
                ctx.Request.Body.Position = 0;          // rewind for MVC
            }
        }

        await _next(ctx);
    }
}
