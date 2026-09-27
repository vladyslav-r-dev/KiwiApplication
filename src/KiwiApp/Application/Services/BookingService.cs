using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases.Bookings.Create;
using KiwiApp.Application.UseCases.Bookings.Get;
using KiwiApp.Application.UseCases.Bookings.Update;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Services;

public class BookingService(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    ILogger<BookingService> logger,
    CacheService cacheService,
    IFlightRepository flightRepository)
{

    public async Task<CreateBookingResult?> CreateBooking(CreateBookingCommand command)
    {
        var flight = await flightRepository.GetFlightById(command.FlightId);

        if (flight is null)
        {
            logger.LogWarning(
                "Flight with id {FlightId} was not found",
                command.FlightId
            );

            throw new KeyNotFoundException(
                $"Flight with id {command.FlightId} was not found"
            );
        }

        var availableSeats = flight.Seats
            .Where(seat => !seat.IsOccupied)
            .ToList();

        if (command.Passengers.Count > availableSeats.Count)
        {
            throw new InvalidOperationException(
                "There are not enough available seats for all passengers."
            );
        }

        var passengers = new List<Passenger>();

        var manualSeatCount = 0;

        foreach (var passengerCommand in command.Passengers)
        {
            Seat selectedSeat;

            if (!string.IsNullOrWhiteSpace(passengerCommand.SelectedSeatNumber))
            {
                selectedSeat = availableSeats.FirstOrDefault(seat =>
                    seat.SeatNumber.Equals(
                        passengerCommand.SelectedSeatNumber,
                        StringComparison.OrdinalIgnoreCase
                    )
                ) ?? throw new InvalidOperationException(
                    $"Seat {passengerCommand.SelectedSeatNumber} is occupied or does not exist."
                );

                manualSeatCount++;
            }
            else
            {
                var randomIndex = Random.Shared.Next(availableSeats.Count);

                selectedSeat = availableSeats[randomIndex];
            }

            selectedSeat.IsOccupied = true;

            availableSeats.Remove(selectedSeat);

            passengers.Add(new Passenger
            {
                FirstName = passengerCommand.FirstName,
                LastName = passengerCommand.LastName,
                SeatId = selectedSeat.Id
            });
        }

        var price =
            flight.Price * passengers.Count +
            15m * manualSeatCount;

        var booking = Booking.CreateBooking(
            command.FlightId,
            passengers,
            command.Email,
            command.UserId,
            price
        );

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

    public async Task<UpdateBookingResult> UpdateBooking(int id, UpdateBookingCommand command)
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

    public async Task<List<AdminBookingResult>> GetAllBookings()
    {
        const string cacheKey = "bookings:all";

        var cachedBookings = await cacheService.GetAsync<List<Booking>>(cacheKey);

        var bookings = cachedBookings;

        if (bookings is null)
        {
            bookings = await bookingRepository.GetAllWithPassengers();

            await cacheService.SetAsync(
                cacheKey,
                bookings,
                TimeSpan.FromMinutes(1)
            );
        }

        return bookings.Select(booking => new AdminBookingResult
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            FlightId = booking.FlightId,
            Email = booking.Email,
            Status = booking.Status.ToString(),
            Price = booking.Price
        }).ToList();
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
                Passengers = cachedBooking.Passengers,
                UserId = cachedBooking.UserId
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
            Passengers = booking.Passengers,
            UserId = booking.UserId
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

    public async Task<List<Booking>> GetBookingByUserId(int userId)
    {
        return await bookingRepository.GetBookingsByUserId(userId);
    }
}