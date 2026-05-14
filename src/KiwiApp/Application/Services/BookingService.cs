using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases.Bookings.Create;
using KiwiApp.Application.UseCases.Bookings.Create.Update;
using KiwiApp.Application.UseCases.Bookings.Get;
using KiwiApp.Application.UseCases.Bookings.Update;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Services;

public class BookingService(
    IGenericRepository<Flight> repository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork, ILogger<BookingService> logger, 
    IEmailService emailService, PdfService pdfService)
{
    public async Task<CreateBookingResult?> Execute(CreateBookingCommand command)
    {
        var flight = await repository.GetById(command.FlightId);

        if (flight is null)
        {
            logger.LogWarning("Flight with id {FlightId} was not found", command.FlightId);

            throw new KeyNotFoundException($"Flight with id {command.FlightId} was not found");
        }
            

        var booking = Booking.CreateBooking(command.FlightId, command.Passengers, command.Email);
        var firstPassenger = command.Passengers.First();
        var clientName = firstPassenger.FirstName;
        
        await bookingRepository.Add(booking);
        await unitOfWork.SaveChangesAsync();
        
        var pdfBytes = pdfService.GenerateBookingPdf(
            clientName,
            booking.Email,
            booking.FlightId
        );

        await emailService.SendBookingConfirmationAsync(
            booking.Email,
            clientName,
            booking.FlightId,
            pdfBytes
        );
        
        return new CreateBookingResult
        {
            BookingId = booking.BookingId,
            Status = booking.Status,
            Price = booking.Price
        };
    }
    
    public async Task DeleteBooking(int id)
    {
        var booking = await bookingRepository.GetByIdWithPassengers(id);
    
        if (booking is null)
        {
            logger.LogWarning("Booking with id {FlightId} was not found", id);

            throw new KeyNotFoundException($"Booking with id {id} was not found");
        }
    
        await bookingRepository.Remove(booking);
            
        await unitOfWork.SaveChangesAsync();
    }
    
    public async Task <UpdateBookingResult> UpdateBooking (int id, UpdateBookingCommand command)
    {
        var booking = await bookingRepository.GetByIdWithPassengers(id);
        
        if (booking is null)
        {
            logger.LogWarning("Booking with id {BookingId} was not found", id);

            throw new KeyNotFoundException($"Booking with id {id} was not found");
        }
        
        booking.UpdateBooking(command.Passengers, command.Email);
        
        await unitOfWork.SaveChangesAsync();
        
        return new UpdateBookingResult
        {
            Passengers = command.Passengers,
            Email = command.Email,
        };
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        return await bookingRepository.GetAllWithPassengers();
    }

    public async Task<GetBookingResult> GetBookingById(int id)
    {
        var booking = await bookingRepository.GetByIdWithPassengers(id);

        if (booking is null)
        {
            logger.LogWarning("Booking with id {BookingId} was not found", id);

            throw new KeyNotFoundException($"Booking with id {id} was not found");
        }

        return new GetBookingResult
        {
            BookingId = booking.BookingId,
            FlightId = booking.FlightId,
            Status = booking.Status,
            Price = booking.Price,
            Email = booking.Email,
            Passengers = booking.Passengers
        };
    }
}