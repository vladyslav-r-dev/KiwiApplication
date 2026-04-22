using KiwiApp.Data;
using KiwiApp.Models;
using KiwiApp.Patterns;
using Microsoft.EntityFrameworkCore;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _db;

    public FlightRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Flight?> GetFlight(Guid id)
    {
        return await _db.Flights
            .FirstOrDefaultAsync(x => x.FlightId == id);
    }

    public Task<List<Flight>> GetAllFlights()
    {
        return _db.Flights.ToListAsync();
    }

    public Task<Flight> AddFlight(Flight flight)
    {
        _db.Flights.Add(flight);
        return Task.FromResult(flight);
    }

    public Task<Flight> RemoveFlight(Flight flight)
    {
        _db.Flights.Remove(flight);
        return Task.FromResult(flight);
    }
}