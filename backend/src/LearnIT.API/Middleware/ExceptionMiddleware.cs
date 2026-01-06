using System.Net;
using System.Text.Json;
using LearnIT.Application.Exceptions;

namespace LearnIT.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var message = exception.Message;
        
        switch (exception)
        {
            case BusinessRuleException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            default:
                _logger.LogError(exception, "Unhandled exception");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                message = "Internal Server Error";
                break;
        }
        
        // Return simple JSON object with message
        var result = JsonSerializer.Serialize(new { message = message });
        await response.WriteAsync(result);
    }
}
