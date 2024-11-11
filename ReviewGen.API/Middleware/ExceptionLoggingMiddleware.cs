using System.Net;
using System.Text.Json;

namespace ReviewGen.API.Middleware;

public class ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var errorResponse = new
        {
            context.Response.StatusCode,
            Message = "An unhandled exception occurred",
            Detailed = ex.Message
        };

        var errorJson = JsonSerializer.Serialize(errorResponse);
        return context.Response.WriteAsync(errorJson);
    }
}