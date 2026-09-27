using KiwiApp.Api.Contracts.Booking;

namespace KiwiApp.Api.Contracts;

public class UpdateBookingRequest
{
    public string Email { get; set; }

    public List<UpdatePassengerRequest> Passengers { get; set; }
}