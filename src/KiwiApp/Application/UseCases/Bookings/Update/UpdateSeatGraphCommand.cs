namespace KiwiApp.Application.UseCases.Bookings.Update;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdateSeatGraphCommand
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public bool IsOccupied { get; set; }
    public UpdateFlightGraphCommand Flight { get; set; } = null!;
}
