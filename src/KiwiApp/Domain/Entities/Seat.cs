
using KiwiApp.Domain.Entities;

public class Seat
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; }
    public bool IsOccupied { get; set; }
    public Flight Flight { get; set; }
}