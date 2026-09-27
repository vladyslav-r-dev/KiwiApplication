namespace KiwiApp.Application.UseCases.Bookings.Update;

public class UpdateBookingCommand
{
    public string Email { get; set; }

    public List<UpdatePassengerCommand> Passengers { get; set; }
}