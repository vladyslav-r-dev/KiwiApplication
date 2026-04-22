using KiwiApp.Models;

namespace KiwiApp.Patterns;

public interface IFlightRepository
{
    Task<Flight?> GetFlight(Guid id);
    Task<List<Flight>> GetAllFlights();
    Task<Flight> AddFlight(Flight flight);
    Task<Flight> RemoveFlight(Flight flight);
}