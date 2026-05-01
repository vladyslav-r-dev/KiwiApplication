using KiwiApp.Domain.Entities;

namespace KiwiApp.Api.Contracts;

public class CreateBookingRequest
{
    public List<Passenger> Passengers { get; set; } = [];
    public Guid FlightId { get; set; }
    public string Email { get; set; }
}