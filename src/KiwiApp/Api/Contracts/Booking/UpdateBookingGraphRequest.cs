using KiwiApp.Domain.Entities;

namespace KiwiApp.Api.Contracts.Booking;

// Preserves the existing HTTP graph, including navigation properties.
public class UpdateBookingGraphRequest
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public UpdateFlightGraphRequest Flight { get; set; } = null!;
    public List<UpdatePassengerRequest> Passengers { get; set; } = null!;
    public BookingStatus Status { get; set; }
    public string? StripeCheckoutSessionId { get; set; }
    public decimal Price { get; set; }
    public string? Email { get; set; }
    public int UserId { get; set; }
}
