namespace KiwiApp.Application.UseCases.Flights.Update;

public class UpdateFlightCommand
{
    public int FlightId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}