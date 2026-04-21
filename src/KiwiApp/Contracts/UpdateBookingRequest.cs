using KiwiApp.Models;

namespace KiwiApp.Contracts;

public class UpdateBookingRequest
{
    public string Email { get; set; }
    public List<Passenger> Passengers { get; set; }
}