namespace KiwiApp.Api.Contracts.Booking;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdatePassengerRequest
{
    public Guid PassengerId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int? SeatId { get; set; }
    public UpdateSeatGraphRequest? Seat { get; set; }
}
