using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Bookings.Create;

public class CreateBookingResult
{
    public int BookingId { get; set; }
    
    public BookingStatus Status { get; set; }
    
    public decimal Price { get; set; }
}