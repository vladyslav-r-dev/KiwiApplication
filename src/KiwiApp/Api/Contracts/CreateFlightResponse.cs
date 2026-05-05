namespace KiwiApp.Api.Contracts;

public class CreateFlightResponse
{
    public int FlightId { get; set; }
    
    public string? From { get; set; }
    
    public string? To { get; set; }
}