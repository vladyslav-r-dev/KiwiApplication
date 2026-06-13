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
    IUnitOfWork unitOfWork, ILogger<BookingService> logger, CacheService cacheService)
{
    public async Task<CreateBookingResult?> CreateBooking(CreateBookingCommand command)
    {
        var flight = await repository.GetById(command.FlightId);

        if (flight is null)
        {
            logger.LogWarning("Flight with id {FlightId} was not found", command.FlightId);

            throw new KeyNotFoundException($"Flight with id {command.FlightId} was not found");
        }
        
        var booking = Booking.CreateBooking(command.FlightId, command.Passengers, command.Email);
        
        await bookingRepository.Add(booking);
        
        await unitOfWork.SaveChangesAsync();
        
        await cacheService.RemoveAsync("bookings:all");
        
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
        
        await cacheService.RemoveAsync("bookings:all");
        await cacheService.RemoveAsync($"bookings:id:{id}");
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
        
        await cacheService.RemoveAsync("bookings:all");
        await cacheService.RemoveAsync($"bookings:id:{booking.BookingId}");
        
        return new UpdateBookingResult
        {
            Passengers = command.Passengers,
            Email = command.Email,
        };
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        const string cacheKey = "bookings:all";

        var cachedBookings = await cacheService.GetAsync<List<Booking>>(cacheKey);
        
        if (cachedBookings is not null)
        {
            return cachedBookings;
        }
        
        var bookings = await bookingRepository.GetAllWithPassengers();

        await cacheService.SetAsync(cacheKey, bookings, TimeSpan.FromMinutes(1));

        return bookings;
    }

    public async Task<GetBookingResult> GetBookingById(int id)
    {
        var cacheKey = $"bookings:id:{id}";

        var cachedBooking = await cacheService.GetAsync<Booking>(cacheKey);
        
        if (cachedBooking is not null)
        {
            return new GetBookingResult
            {
                BookingId = cachedBooking.BookingId,
                FlightId = cachedBooking.FlightId,
                Status = cachedBooking.Status,
                Price = cachedBooking.Price,
                Email = cachedBooking.Email,
                Passengers = cachedBooking.Passengers
            };
        }
        
        var booking = await bookingRepository.GetByIdWithPassengers(id);

        if (booking is null)
        {
            logger.LogWarning("Booking with id {BookingId} was not found", id);

            throw new KeyNotFoundException($"Booking with id {id} was not found");
        }

        await cacheService.SetAsync(cacheKey, booking, TimeSpan.FromMinutes(1));

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

    public async Task MarkAsPaid(int id)
    {
        var booking = await bookingRepository.GetByIdWithPassengers(id);
        
        if (booking is null)
            return;

        if (booking.Status == BookingStatus.Paid)
            return;

        booking.Status = BookingStatus.Paid;
        
        await unitOfWork.SaveChangesAsync();
        
        await cacheService.RemoveAsync("bookings:all");
    }
    
    public async Task SetStripeCheckoutSessionId(int bookingId, string sessionId)
    {
        var booking = await bookingRepository.GetById(bookingId);

        if (booking is null)
        {
            logger.LogWarning("Booking with id {BookingId} was not found", bookingId);
            throw new KeyNotFoundException($"Booking with id {bookingId} was not found");
        }

        booking.StripeCheckoutSessionId = sessionId;

        await unitOfWork.SaveChangesAsync();
        
        await cacheService.RemoveAsync("bookings:all");
        await cacheService.RemoveAsync($"bookings:id:{bookingId}");
    }
    
    public async Task<List<Booking>> GetPendingBookingsWithStripeSession()
    {
        return await bookingRepository.GetPendingBookingsWithStripeSession();
    }
}