namespace KiwiApp.Api.ErrorHandling;

public sealed class ErrorResponse
{
    public int StatusCode { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Path { get; init; }
    public IReadOnlyDictionary<string, string[]>? Errors { get; init; }
}