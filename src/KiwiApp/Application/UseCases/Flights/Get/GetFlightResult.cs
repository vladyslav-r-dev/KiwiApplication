namespace KiwiApp.Application.UseCases.Flights.Get;

public class GetFlightResult
{
    public int FlightId { get; set; }

    public string? From { get; set; }

    public string? To { get; set; }

    public string? FromIata { get; set; }

    public string? ToIata { get; set; }

    public string? Airline { get; set; }

    public string? FlightNumber { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public decimal Price { get; set; }

    public string? Status { get; set; }

    public List<SeatResult> Seats { get; set; } = [];
}