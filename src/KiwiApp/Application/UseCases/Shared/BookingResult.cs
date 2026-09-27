using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Shared;

// Preserves the existing HTTP graph, including navigation properties.
public class BookingResult
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public FlightResult Flight { get; set; } = null!;
    public List<PassengerResult> Passengers { get; set; } = null!;
    public BookingStatus Status { get; set; }
    public string? StripeCheckoutSessionId { get; set; }
    public decimal Price { get; set; }
    public string? Email { get; set; }
    public int UserId { get; set; }
}
