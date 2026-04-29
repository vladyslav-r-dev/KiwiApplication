namespace KiwiApp.Application.UseCases.Flights;

public class CreateFlightResult
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}