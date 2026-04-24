using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Patterns;

public class FlightRepository(AppDbContext db) : IFlightRepository
{
    public async Task<Flight?> GetFlight(Guid id)
    {
        return await db.Flights
            .FirstOrDefaultAsync(x => x.FlightId == id);
    }

    public Task<List<Flight>> GetAllFlights()
    {
        return db.Flights.ToListAsync();
    }

    public Task<Flight> AddFlight(Flight flight)
    {
        db.Flights.Add(flight);
        return Task.FromResult(flight);
    }

    public Task<Flight> RemoveFlight(Flight flight)
    {
        db.Flights.Remove(flight);
        return Task.FromResult(flight);
    }
}