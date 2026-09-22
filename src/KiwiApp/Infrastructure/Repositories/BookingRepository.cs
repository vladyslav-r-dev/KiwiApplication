using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases;
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

    public Task<List<Booking>> GetBookingsByID(int userId)
    {
        return db.Bookings
        .Where(booking => booking.UserId == userId)
        .ToListAsync();
    }
}