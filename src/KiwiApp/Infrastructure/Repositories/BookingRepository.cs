using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Repositories;

public class BookingRepository(AppDbContext db)
    : GenericRepository<Booking>(db), IBookingRepository
{
    public Task<Booking?> GetByIdWithPassengers(int id)
    {
        return db.Bookings
            .Include(x => x.Passengers)
            .ThenInclude(p => p.Seat)
            .FirstOrDefaultAsync(x => x.BookingId == id);
    }

    public Task<List<Booking>> GetAllWithPassengers()
    {
        return db.Bookings
            .Include(x => x.Passengers)
            .ToListAsync();
    }

    public Task<List<Booking>> GetPendingBookingsWithStripeSession()
    {
        return db.Bookings
            .Where(b => b.Status == BookingStatus.Pending)
            .Where(b => b.StripeCheckoutSessionId != null)
            .ToListAsync();
    }

    public async Task<bool> TryMarkAsPaid(int id)
    {
        var updated = await db.Bookings
            .Where(booking => booking.BookingId == id && booking.Status != BookingStatus.Paid)
            .ExecuteUpdateAsync(update => update.SetProperty(booking => booking.Status, BookingStatus.Paid));

        return updated == 1;
    }

    public async Task ReleaseSeatsForBooking(Booking booking)
    {
        var seats = booking.Passengers
            .Where(passenger => passenger.Seat is not null)
            .Select(passenger => passenger.Seat!)
            .DistinctBy(seat => seat.Id)
            .ToList();
        var seatIds = seats.Select(seat => seat.Id).ToList();
        var seatsUsedElsewhere = await db.Bookings
            .Where(other => other.BookingId != booking.BookingId)
            .SelectMany(other => other.Passengers)
            .Where(passenger => passenger.SeatId.HasValue && seatIds.Contains(passenger.SeatId.Value))
            .Select(passenger => passenger.SeatId!.Value)
            .ToListAsync();

        foreach (var seat in seats)
        {
            if (!seatsUsedElsewhere.Contains(seat.Id))
                seat.IsOccupied = false;
        }
    }

    public Task<List<Booking>> GetBookingsByUserId(int userId)
    {
        return db.Bookings
        .Where(booking => booking.UserId == userId)
        .ToListAsync();
    }
}