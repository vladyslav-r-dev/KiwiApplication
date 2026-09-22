using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Repositories;

public class FlightRepository(AppDbContext db) : GenericRepository<Flight>(db), IFlightRepository
{
    private readonly AppDbContext _db = db;

   public async Task<List<Flight>> GetFilteredFlights(
    string? from,
    string? to,
    string? status,
    decimal? minPrice,
    decimal? maxPrice,
    string? sortBy,
    string? airline,
    string? departureDate)
{
    var query = _db.Flights.AsQueryable();

    if (!string.IsNullOrWhiteSpace(from))
    {
        query = query.Where(x =>
            x.From == from ||
            x.FromIata == from);
    }

    if (!string.IsNullOrWhiteSpace(to))
    {
        query = query.Where(x =>
            x.To == to ||
            x.ToIata == to);
    }

    if (!string.IsNullOrWhiteSpace(status))
    {
        query = query.Where(x => x.Status == status);
    }

    if (!string.IsNullOrWhiteSpace(airline))
    {
        query = query.Where(x => x.Airline == airline);
    }

    if (minPrice.HasValue)
    {
        query = query.Where(x => x.Price >= minPrice.Value);
    }

    if (maxPrice.HasValue)
    {
        query = query.Where(x => x.Price <= maxPrice.Value);
    }

    if (!string.IsNullOrWhiteSpace(departureDate) &&
        DateTime.TryParse(departureDate, out var date))
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        query = query.Where(x =>
            x.DepartureTime.HasValue &&
            x.DepartureTime.Value >= startOfDay &&
            x.DepartureTime.Value < endOfDay);
    }

    query = sortBy switch
    {
        "priceAsc" => query.OrderBy(x => x.Price),
        "priceDesc" => query.OrderByDescending(x => x.Price),
        "departureAsc" => query.OrderBy(x => x.DepartureTime),
        "departureDesc" => query.OrderByDescending(x => x.DepartureTime),
        _ => query
    };

    return await query.ToListAsync();
}

    public async Task<List<Flight>> GetFlightFromTo(string? from, string? to)
    {
        var search = _db.Flights.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(from))
        {
            search = search.Where(x => x.From == from || x.FromIata == from);
        }

        if (!string.IsNullOrWhiteSpace(to))
        {
            search = search.Where(x => x.To == to || x.ToIata == to);
        }
        
        return await search.ToListAsync();
    }

    public async Task<Flight?> GetFlightById(int id)
    {
    return await _db.Flights
        .Include(f => f.Seats)
        .FirstOrDefaultAsync(f => f.FlightId == id);
    }
}