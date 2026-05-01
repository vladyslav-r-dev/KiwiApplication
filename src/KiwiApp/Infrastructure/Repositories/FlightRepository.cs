using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Repositories;

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

    public async Task<List<Flight>> GetFlightFromTo(string from, string to)
    {
        var search = db.Flights.AsQueryable();
        if (!string.IsNullOrWhiteSpace(from))
        {
            search = search.Where(x => x.From == from);
        }

        if (!string.IsNullOrWhiteSpace(to))
        {
            search = search.Where(x => x.To == to);
        }
        
        return await search.ToListAsync();
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