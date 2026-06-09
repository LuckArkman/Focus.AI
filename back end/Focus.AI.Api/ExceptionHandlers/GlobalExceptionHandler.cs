using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Focus.AI.Api.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorResponse = new ErrorResponse
        {
            Message = "An unexpected error occurred."
        };

        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            errorResponse.Message = "Validation failed.";
            errorResponse.Errors = validationException.Errors.Select(e => e.ErrorMessage);
            
            _logger.LogWarning(exception, "Validation failed: {Message}", exception.Message);
        }
        else if (exception is UnauthorizedAccessException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            errorResponse.Message = "Unauthorized access.";
            
            _logger.LogWarning(exception, "Unauthorized access: {Message}", exception.Message);
        }
        else
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError(exception, "A severe error occurred: {Message}", exception.Message);
        }

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; }
}
