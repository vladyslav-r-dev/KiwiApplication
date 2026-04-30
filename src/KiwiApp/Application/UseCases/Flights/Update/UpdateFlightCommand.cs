namespace KiwiApp.Application.UseCases.Flights.Update;

public class UpdateFlightCommand
{
    public Guid FlightId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}