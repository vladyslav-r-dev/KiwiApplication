namespace KiwiApp.Application.UseCases.Shared;

// Preserves the existing HTTP graph, including navigation properties.
public class PassengerResult
{
    public Guid PassengerId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int? SeatId { get; set; }
    public SeatDetailsResult? Seat { get; set; }
}
