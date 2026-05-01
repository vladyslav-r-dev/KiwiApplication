using KiwiApp.Domain.Enums;

namespace KiwiApp.Application.UseCases.Bookings.Create;

public class CreateBookingResult
{
    public Guid BookingId { get; set; }
    public BookingStatus Status { get; set; }
    public int Price { get; set; }
}