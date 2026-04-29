using KiwiApp.Models;

namespace KiwiApp.Application.UseCases.Bookings;

public class CreateBookingCommand
{
    public Guid FlightId { get; set; }
    public List<Passenger> Passengers { get; set; } = [];
    public string Email { get; set; }
}