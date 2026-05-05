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
            .FirstOrDefaultAsync(x => x.BookingId == id);
    }

    public Task<List<Booking>> GetAllWithPassengers()
    {
        return db.Bookings
            .Include(x => x.Passengers)
            .ToListAsync();
    }
}