namespace KiwiApp.Application.UseCases.Flights.Get;

public class GetFlightResult
{
    public int FlightId { get; set; }
    
    public string? From { get; set; }
    
    public string? To { get; set; }
}