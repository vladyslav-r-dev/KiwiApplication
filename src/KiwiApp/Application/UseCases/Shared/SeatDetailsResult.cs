namespace KiwiApp.Application.UseCases.Shared;

// Preserves the existing HTTP graph, including navigation properties.
public class SeatDetailsResult
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public bool IsOccupied { get; set; }
    public FlightResult Flight { get; set; } = null!;
}
