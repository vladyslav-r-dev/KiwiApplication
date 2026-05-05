namespace KiwiApp.Domain.Exceptions;

public sealed class NotFoundException(string message) : DomainException(message);