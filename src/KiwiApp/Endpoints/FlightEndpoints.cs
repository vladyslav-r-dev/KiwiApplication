using FluentValidation;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;
using KiwiApp.Validator;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Endpoints;

public static class FlightEndpoints
{
    public static void MapFlightEndpoints(this WebApplication app)
    {
        app.MapGet("/flights", async (AppDbContext db) =>
        {
            return await db.Flights.ToListAsync();
        });

        app.MapGet("/flights/{id}", async (Guid id, AppDbContext db) =>
        {
            var flightId = db.Flights.FirstOrDefault(x => x.FlightId == id);
            if (flightId is null)
            {
                return Results.NotFound();
            }
            
            return Results.Ok(flightId);
        });

        app.MapPost("/flights", async (Flight flight, AppDbContext db) =>
        {
            // тут может добавить валидацию так же на проверку ИД введенего полета?
            flight.FlightId = Guid.NewGuid();
    
            db.Flights.Add(flight);
            await db.SaveChangesAsync();
            return Results.Created($"/flights/{flight.FlightId}", flight);
        });

        app.MapPut("/flights/{id}", async (Guid id, UpdateFlightRequest request, IValidator<UpdateFlightRequest> validator, AppDbContext db) =>
        {
            var flight = await db.Flights.FirstOrDefaultAsync(x => x.FlightId == id);
            if (flight is null)
            {
                return Results.NotFound();
            }
            
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
            
            flight.From = request.From;
            flight.To = request.To;
            await db.SaveChangesAsync();
    
            return Results.Ok(flight);
        });

        app.MapDelete("/flights/{id}", async (Guid id, AppDbContext db) =>
        {
            var flightId = await db.Flights.FirstOrDefaultAsync(x => x.FlightId == id);
            if (flightId is null)
            {
                return Results.NotFound();
            }
            db.Flights.Remove(flightId);
            await db.SaveChangesAsync();
    
            return Results.NoContent();
        });
    }

}