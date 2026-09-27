using KiwiApp.Application.Exceptions;
using KiwiApp.Domain.Exceptions;

namespace KiwiApp.Api.ErrorHandling;

public static class ExceptionMapper
{
    public static ErrorResponse Map(Exception exception, string? path)
    {
        return exception switch
        {
            ValidationException ex => new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Message = ex.Message,
                Path = path,
                Errors = ex.Errors
            },

            BadRequestException ex => Create(
                StatusCodes.Status400BadRequest,
                "Bad request",
                ex.Message,
                path),

            NotFoundException ex => Create(
                StatusCodes.Status404NotFound,
                "Not found",
                ex.Message,
                path),

            ConflictException ex => Create(
                StatusCodes.Status409Conflict,
                "Conflict",
                ex.Message,
                path),

            ForbiddenException ex => Create(
                StatusCodes.Status403Forbidden,
                "Forbidden",
                ex.Message,
                path),

            KeyNotFoundException ex => Create(
                StatusCodes.Status404NotFound,
                "Not found",
                ex.Message,
                path),

            UnauthorizedAccessException ex => Create(
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                ex.Message,
                path),

            _ => Create(
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "Unexpected server error",
                path)
        };
    }

    private static ErrorResponse Create(
        int statusCode,
        string title,
        string message,
        string? path)
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Title = title,
            Message = message,
            Path = path
        };
    }
}