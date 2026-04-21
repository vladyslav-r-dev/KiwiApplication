using KiwiApp.Models;

namespace KiwiApp.Data;

public class AppData
{
    public List<Booking> Bookings { get; set; } = new();
    public List<Flight> Flights { get; set; } = new();
}