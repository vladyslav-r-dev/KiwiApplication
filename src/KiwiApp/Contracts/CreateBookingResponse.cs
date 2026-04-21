using KiwiApp.Models;

namespace KiwiApp.Contracts;

public class CreateBookingResponse
{
    public Guid BookingId { get; set; }
    public BookingStatus Status { get; set; }
    public int Price { get; set; }
}