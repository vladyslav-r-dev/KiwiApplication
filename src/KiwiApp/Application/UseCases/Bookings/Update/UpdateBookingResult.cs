using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Bookings.Create.Update;

public class UpdateBookingResult
{
    public string Email { get; set; }
    
    public List<Passenger> Passengers { get; set; }
}