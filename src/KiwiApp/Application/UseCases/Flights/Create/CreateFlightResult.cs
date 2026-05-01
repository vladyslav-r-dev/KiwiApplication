namespace KiwiApp.Application.UseCases.Flights.Create;

public class CreateFlightResult
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}