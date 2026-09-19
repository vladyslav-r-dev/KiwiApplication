using KiwiApp.Domain.Entities;

namespace KiwiApp.Api.Contracts.Booking;

public class CreateBookingResponse
{
    public int BookingId { get; set; }
    
    public BookingStatus Status { get; set; }
    
    public decimal Price { get; set; }
    
    public string? CheckoutUrl { get; set; }
}