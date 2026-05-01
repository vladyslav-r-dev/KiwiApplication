using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Bookings.Create;

public class CreateBookingUseCase(
    IFlightRepository flightRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<CreateBookingResult?> Execute(CreateBookingCommand command)
    {
        var flight = await flightRepository.GetFlight(command.FlightId);
        if (flight is null)
            return null;

        var booking = Booking.CreateBooking(command.FlightId, command.Passengers, command.Email);

        await bookingRepository.AddBooking(booking);
        await unitOfWork.SaveChangesAsync();
    
        return new CreateBookingResult
        {
            BookingId = booking.BookingId,
            Status = booking.Status,
            Price = booking.Price
        };
    }
}