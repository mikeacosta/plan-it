using System.Net;

namespace PlanIt.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleGlobalException(context, ex);
        }
    }

    private Task HandleGlobalException(HttpContext context, Exception ex)
    {
        if (ex is ApplicationException)
        {
            _logger.LogWarning($"Validation error occurred in API. {ex.Message}");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsJsonAsync(new { ex.Message });
        }

        var errorId = Guid.NewGuid();
        _logger.LogError(ex, $"Exception occurred in API: {errorId}");
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsJsonAsync(new
        {
            ErrorId = errorId,
            Message = $"Something bad happened in our API. Error ID: {errorId}"
        });
    }
}