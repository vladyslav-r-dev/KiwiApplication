namespace KiwiApp.Application.UseCases.Bookings.Update;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdatePassengerCommand
{
    public Guid PassengerId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int? SeatId { get; set; }
    public UpdateSeatGraphCommand? Seat { get; set; }
}
