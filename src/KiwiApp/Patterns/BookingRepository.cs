using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Patterns;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _db;
    
    public BookingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Booking?> GetBooking(Guid id)
    {
        return await _db.Bookings
            .Include(x => x.Passengers)
            .FirstOrDefaultAsync(x => x.BookingId == id);
    }

    public Task<List<Booking>> GetAllBookings()
    {
        return _db.Bookings
            .Include(x => x.Passengers)
            .ToListAsync();
    }

    public Task<Booking> AddBooking(Booking booking)
    {
        _db.Bookings.Add(booking);
        return Task.FromResult(booking);
    }

    public Task<Booking> RemoveBooking(Booking booking)
    {
        _db.Bookings.Remove(booking);
        return Task.FromResult(booking);
    }
}