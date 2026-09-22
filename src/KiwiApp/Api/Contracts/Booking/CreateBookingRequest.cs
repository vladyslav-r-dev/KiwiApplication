using KiwiApp.Domain.Entities;

namespace KiwiApp.Api.Contracts;

public class CreateBookingRequest
{
    public List<CreatePassengerRequest> Passengers { get; set; } = [];
    
    public int FlightId { get; set; }
    
    public string Email { get; set; }
}