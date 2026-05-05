using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Repositories;

public class FlightRepository(AppDbContext db) : GenericRepository<Flight>(db), IFlightRepository
{
    private readonly AppDbContext _db = db;

    public async Task<List<Flight>> GetFlightFromTo(string from, string to)
    {
        var search = _db.Flights.AsQueryable();
        
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
}