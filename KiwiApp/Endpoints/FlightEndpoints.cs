using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;

namespace KiwiApp.Endpoints;

public static class FlightEndpoints
{
    public static void MapFlightEndpoints(this WebApplication app, AppData appData)
    {
        app.MapGet("/flights", () => appData.Flights);

        app.MapGet("/flights/{id}", (Guid id) =>
        {
            var flightId = appData.Flights.FirstOrDefault(x => x.FlightId == id);
            if (flightId is null)
            {
                return Results.NotFound();
            }
    
            return Results.Ok(flightId);
        });

        app.MapPost("/flights", (Flight flight) =>
        {
            flight.FlightId = Guid.NewGuid();
    
            appData.Flights.Add(flight);
            return Results.Created($"/flights/{flight.FlightId}", flight);
        });

        app.MapPut("/flights/{id}", (Guid id, UpdateFlightRequest request) =>
        {
            var flight = appData.Flights.FirstOrDefault(x => x.FlightId == id);
            if (flight is null)
            {
                return Results.NotFound();
            }

            flight.From = request.From;
            flight.To = request.To;
    
            return Results.Ok(flight);
        });

        app.MapDelete("/flights/{id}", (Guid id) =>
        {
            var flightId = appData.Flights.FirstOrDefault(x => x.FlightId == id);
            if (flightId is null)
            {
                return Results.NotFound();
            }
            appData.Flights.Remove(flightId);
    
            return Results.NoContent();
        });
    }

}