using WebAPI.Exceptions;

namespace WebAPI.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Achtung!");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            MaxSubscriptionsLimitException => (StatusCodes.Status409Conflict, exception.Message),
            _ => (StatusCodes.Status409Conflict, "Sorry we have a problem. Somebody will be fired :)")
        };
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message
        };
        
        if (_env.IsDevelopment())
        {
            response.ExceptionMessage = exception.Message;
            response.StackTrace = exception.StackTrace;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}