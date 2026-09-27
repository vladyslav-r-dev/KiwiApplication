namespace KiwiApp.Api.Contracts.Booking;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdateFlightGraphRequest
{
    public int FlightId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public List<UpdateBookingGraphRequest> Bookings { get; set; } = [];
    public string? FromIata { get; set; }
    public string? ToIata { get; set; }
    public string? Airline { get; set; }
    public string? FlightNumber { get; set; }
    public DateTime? DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public string? Status { get; set; }
    public decimal Price { get; set; }
    public List<UpdateSeatGraphRequest> Seats { get; set; } = [];
}
