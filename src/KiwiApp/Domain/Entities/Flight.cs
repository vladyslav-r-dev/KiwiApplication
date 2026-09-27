namespace KiwiApp.Domain.Entities;

public class Flight
{
    public int FlightId { get; set; }

    public string? From { get; set; }

    public string? To { get; set; }

    public List<Booking> Bookings { get; set; } = [];

    public string? FromIata { get; set; }

    public string? ToIata { get; set; }

    public string? Airline { get; set; }

    public string? FlightNumber { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public string? Status { get; set; }

    public decimal Price { get; set; }

    public List<Seat> Seats { get; set; } = [];

    public static Flight CreateFlight(string? from, string? to)
    {
        return new Flight
        {
            FlightId = new Random().Next(),
            From = from,
            To = to,
            FromIata = null,
            ToIata = null,
            Airline = null,
            FlightNumber = null,
            DepartureTime = null,
            ArrivalTime = null,
            Status = null,
            Price = 0
        };
    }

    public void UpdateFlight(string? from, string? to)
    {
        From = from;
        To = to;
    }
}