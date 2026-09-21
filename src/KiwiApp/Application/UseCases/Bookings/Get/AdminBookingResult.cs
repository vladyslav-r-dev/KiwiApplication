namespace KiwiApp.Application.UseCases.Bookings.Get;

public class AdminBookingResult
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int FlightId { get; set; }
    public required string Email { get; set; }
    public required string Status { get; set; }
    public decimal Price { get; set; }
}