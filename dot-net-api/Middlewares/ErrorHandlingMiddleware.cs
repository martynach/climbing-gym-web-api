using dot_net_api.Exceptions;

namespace dot_net_api.Middlewares;

public class ErrorHandlingMiddleware: IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _logger.LogDebug("Beginning of error handling middleware");
            await next(context);
            _logger.LogDebug("End of error handling middleware - no errors");
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            _logger.LogDebug($"Not found exception: {notFound.Message}");
            await context.Response.WriteAsync(notFound.Message);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = 500;
            _logger.LogError($"Error: {exception.Message}", exception);
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}