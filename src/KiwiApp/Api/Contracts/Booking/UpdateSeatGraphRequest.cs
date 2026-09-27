namespace KiwiApp.Api.Contracts.Booking;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdateSeatGraphRequest
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public bool IsOccupied { get; set; }
    public UpdateFlightGraphRequest Flight { get; set; } = null!;
}
