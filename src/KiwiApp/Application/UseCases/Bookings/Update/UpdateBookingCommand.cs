using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Bookings.Update;

public class UpdateBookingCommand
{
    public string Email { get; set; }

    public List<Passenger> Passengers { get; set; }
}