namespace KiwiApp.Application.UseCases.Shared;

// Preserves the existing HTTP graph, including navigation properties.
public class FlightResult
{
    public int FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public List<BookingResult> Bookings { get; set; } = [];
    public string? FromIata { get; set; }
    public string? ToIata { get; set; }
    public string? Airline { get; set; }
    public string? FlightNumber { get; set; }
    public DateTime? DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public string? Status { get; set; }
    public decimal Price { get; set; }
    public List<SeatDetailsResult> Seats { get; set; } = [];
}
