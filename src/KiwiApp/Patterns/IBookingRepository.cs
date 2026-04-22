using KiwiApp.Models;

namespace KiwiApp.Patterns;

public interface IBookingRepository
{
    Task<Booking?> GetBooking(Guid id);
    Task<List<Booking>> GetAllBookings();
    Task<Booking> AddBooking(Booking booking);
    Task<Booking> RemoveBooking(Booking booking);
}