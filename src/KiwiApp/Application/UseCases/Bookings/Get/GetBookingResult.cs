using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Bookings.Get;

public class GetBookingResult
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public List<Passenger> Passengers { get; set; }
    public BookingStatus Status { get; set; }
    public decimal Price { get; set; }
    public string? Email { get; set; }
    public int UserId { get; set; }
}