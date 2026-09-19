namespace KiwiApp.Application.Exceptions;

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errors) : Exception("Validation failed")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}