using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Bookings.Update;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdateBookingGraphCommand
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public UpdateFlightGraphCommand Flight { get; set; } = null!;
    public List<UpdatePassengerCommand> Passengers { get; set; } = null!;
    public BookingStatus Status { get; set; }
    public string? StripeCheckoutSessionId { get; set; }
    public decimal Price { get; set; }
    public string? Email { get; set; }
    public int UserId { get; set; }
}
