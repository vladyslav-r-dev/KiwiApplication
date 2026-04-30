using KiwiApp.Models;

namespace KiwiApp.Application.UseCases.Bookings;

public class CreateBookingResult
{
    public Guid BookingId { get; set; }
    public BookingStatus Status { get; set; }
    public int Price { get; set; }
}