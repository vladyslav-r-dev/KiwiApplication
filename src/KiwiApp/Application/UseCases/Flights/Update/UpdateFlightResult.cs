namespace KiwiApp.Application.UseCases.Flights.Update;

public class UpdateFlightResult
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}