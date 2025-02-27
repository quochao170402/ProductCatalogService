using Microsoft.AspNetCore.Authentication;
using ProductCatalogService.Controllers.Common;
using ProductCatalogService.Exceptions;

namespace ProductCatalogService.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ResultDto
            {
                Data = null,
                Status = StatusCodes.Status500InternalServerError,
                Message = "Server Error"
            };

            switch (exception)
            {
                case EntityNotFoundException entityNotFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Message = entityNotFoundException.Message ?? "The requested entity was not found.";
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Message = "An unexpected error occurred on the server.";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
