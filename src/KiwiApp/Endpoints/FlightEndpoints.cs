using FluentValidation;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;
using KiwiApp.Patterns;

namespace KiwiApp.Endpoints;

public static class FlightEndpoints
{
    public static void MapFlightEndpoints(this WebApplication app)
    {
        app.MapGet("/flights", async (IFlightRepository flightRepository) =>
        {
            return await flightRepository.GetAllFlights();
        });

        app.MapGet("/flights/{id}", async (Guid id, IFlightRepository flightRepository) =>
        {
            var flightId = flightRepository.GetFlight(id);

            return Results.Ok(flightId);
        });

        app.MapPost("/flights", async (Flight flight, IFlightRepository flightRepository, AppDbContext db) =>
        {
            // тут может добавить валидацию так же на проверку ИД введенего полета?
            flight.FlightId = Guid.NewGuid();
    
            flightRepository.AddFlight(flight);
            await db.SaveChangesAsync();
            return Results.Created($"/flights/{flight.FlightId}", flight);
        });

        app.MapPut("/flights/{id}", async (Guid id, UpdateFlightRequest request, 
            IValidator<UpdateFlightRequest> validator, AppDbContext db, IFlightRepository flightRepository) =>
        {
            var flight = await flightRepository.GetFlight(id);
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

        app.MapDelete("/flights/{id}", async (Guid id, AppDbContext db, IFlightRepository flightRepository) =>
        {
            var flightId = await flightRepository.GetFlight(id);
            if (flightId is null)
            {
                return Results.NotFound();
            }
            
            await flightRepository.RemoveFlight(flightId);
            await db.SaveChangesAsync();
    
            return Results.NoContent();
        });
    }

}