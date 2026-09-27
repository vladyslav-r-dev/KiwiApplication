using KiwiApp.Application.UseCases.Shared;

namespace KiwiApp.Application.UseCases.Bookings.Update;

public class UpdateBookingResult
{
    public string Email { get; set; }

    public List<PassengerResult> Passengers { get; set; }
}