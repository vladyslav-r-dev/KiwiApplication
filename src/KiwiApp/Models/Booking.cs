namespace KiwiApp.Models;

public class Booking
{
    public Guid BookingId { get; set; }
    public Guid FlightId { get; set; }
    public List<Passenger> Passengers { get; set; }
    public BookingStatus Status { get; set; }
    public int Price { get; set; }
    public string Email { get; set; }
}