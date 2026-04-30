namespace KiwiApp.Application.UseCases.Flights;

public class GetFlightResult
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}