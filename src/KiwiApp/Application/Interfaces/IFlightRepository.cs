using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IFlightRepository
{
    Task<Flight?> GetFlight(Guid id);
    Task<List<Flight>> GetAllFlights();
    Task<List<Flight>> GetFlightFromTo(string from, string to);
    Task<Flight> AddFlight(Flight flight);
    Task<Flight> RemoveFlight(Flight flight);
}