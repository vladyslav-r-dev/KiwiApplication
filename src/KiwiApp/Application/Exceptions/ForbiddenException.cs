namespace KiwiApp.Application.Exceptions;

public sealed class ForbiddenException(string message) : Exception(message);