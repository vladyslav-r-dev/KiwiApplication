namespace KiwiApp.Domain.Entities;

public class Flight
{
    public int FlightId { get; set; }
    
    public string? From { get; set; }
    
    public string? To { get; set; }
    
    public List<Booking> Bookings { get; set; } = [];

    public static Flight CreateFlight(string? from, string? to)
    {
        return new Flight
        {
            FlightId = new Random().Next(),
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