using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<Booking?> GetByIdWithPassengers(int id);
    Task<List<Booking>> GetAllWithPassengers();
    Task<List<Booking>> GetPendingBookingsWithStripeSession();
    Task<List<Booking>> GetBookingsByUserId(int userId);
    Task<bool> TryMarkAsPaid(int id);
    Task ReleaseSeatsForBooking(Booking booking);
}