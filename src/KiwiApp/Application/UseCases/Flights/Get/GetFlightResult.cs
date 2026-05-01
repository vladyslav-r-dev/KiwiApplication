namespace KiwiApp.Application.UseCases.Flights.Get;

public class GetFlightResult
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}