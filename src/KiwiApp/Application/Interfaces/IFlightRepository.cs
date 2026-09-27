using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IFlightRepository : IGenericRepository<Flight>
{
    Task<List<Flight>> GetFlightFromTo(string? from, string? to);
    Task<List<Flight>> GetFilteredFlights(string? from, string? to, string? status,
    decimal? minPrice, decimal? maxPrice, string? sortBy, string? airline, string? departureDate);
    Task<Flight?> GetFlightById(int id);
}