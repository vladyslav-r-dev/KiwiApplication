using KiwiApp.Domain.Entities;
using KiwiApp.Application.Services;

namespace KiwiApp.Application.Interfaces;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<Booking?> GetByIdWithPassengers(int id);
    Task<List<Booking>> GetAllWithPassengers();
    Task<List<Booking>> GetPendingBookingsWithStripeSession();
    Task<List<Booking>> GetBookingsByID(int userId);
}