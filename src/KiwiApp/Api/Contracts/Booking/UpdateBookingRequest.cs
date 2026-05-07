using KiwiApp.Domain.Entities;

namespace KiwiApp.Api.Contracts;

public class UpdateBookingRequest
{
    public string Email { get; set; }
    
    public List<Passenger> Passengers { get; set; }
}