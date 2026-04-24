using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Patterns;

public class BookingRepository(AppDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetBooking(Guid id)
    {
        return await db.Bookings
            .Include(x => x.Passengers)
            .FirstOrDefaultAsync(x => x.BookingId == id);
    }

    public Task<List<Booking>> GetAllBookings()
    {
        return db.Bookings
            .Include(x => x.Passengers)
            .ToListAsync();
    }

    public Task<Booking> AddBooking(Booking booking)
    {
        db.Bookings.Add(booking);
        return Task.FromResult(booking);
    }

    public Task<Booking> RemoveBooking(Booking booking)
    {
        db.Bookings.Remove(booking);
        return Task.FromResult(booking);
    }
}