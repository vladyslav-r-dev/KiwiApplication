
namespace KiwiApp.Application.UseCases.Bookings.Create;

public class CreateBookingCommand
{
    public int FlightId { get; set; }

    public List<CreatePassengerCommand> Passengers { get; set; } = [];

    public string Email { get; set; }

    public int UserId { get; set; }
}