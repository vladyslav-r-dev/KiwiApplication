namespace KiwiApp.Contracts;

public class CreateFlightResponse
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
}