namespace KiwiApp.Domain.Entities;

public class Booking
{
    public int BookingId { get; set; }
    
    public int FlightId { get; set; }
    
    public Flight Flight { get; set ;}
    
    public List<Passenger> Passengers { get; set; }
    
    public BookingStatus Status { get; set; }
    
    public string? StripeCheckoutSessionId { get; set; }
    
    public decimal Price { get; set; }
    
    public string? Email { get; set; }

    public int UserId { get; set; }    

    public static Booking CreateBooking(
        int flightId,
        List<Passenger> passengers,
        string email, int userId)
    {
        return new Booking
        {
            BookingId = new Random().Next(),
            FlightId = flightId,
            Passengers = passengers,
            Price = 100,
            Email = email,
            Status = BookingStatus.Pending,
            UserId = userId
        };
    }
    
    public void UpdateBooking(List<Passenger> passengers, string? email)
    {
        Passengers = passengers;
        Email = email;
    }
}