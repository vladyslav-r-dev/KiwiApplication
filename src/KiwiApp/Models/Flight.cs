namespace KiwiApp.Models;

public class Flight
{
    public Guid FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }

    public static Flight CreateFlight(string? from, string? to)
    {
        return new Flight
        {
            FlightId = Guid.NewGuid(),
            From = from,
            To = to
        };
    }

    public void UpdateFlight(string? from, string? to)
    {
        From = from;
        To = to;
    }
}