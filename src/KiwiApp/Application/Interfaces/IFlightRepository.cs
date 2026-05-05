using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IFlightRepository : IGenericRepository<Flight>
{
    Task<List<Flight>> GetFlightFromTo(string from, string to);
}