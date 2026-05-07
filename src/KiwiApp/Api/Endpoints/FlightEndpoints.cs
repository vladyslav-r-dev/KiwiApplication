using FluentValidation;
using KiwiApp.Api.Contracts;
using KiwiApp.Application.Interfaces;
using KiwiApp.Application.Services;
using KiwiApp.Application.UseCases.Flights.Create;
using KiwiApp.Application.UseCases.Flights.Update;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Api.Endpoints;

public static class FlightEndpoints
{
    public static void MapFlightEndpoints(this WebApplication app)
    {
        app.MapGet("/flights", async (FlightService service) =>
        {
            var result = await service.GetAllFlights();

            return Results.Ok(result);
        });

        app.MapGet("/flights/{id:int}", async (FlightService service, int id) =>
        {
            var result = await service.GetAllFlightById(id);

            return Results.Ok(result);
        });
        
        app.MapGet("/flights/import/api", async (FlightService service) =>
        {
            await service.ImportFlights();
            
            return Results.Ok("Flights imported");
            
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        app.MapGet("/search/flight", async (FlightService service, string from, string to) =>
        {
            var result = await service.SearchFlight(from, to);
            
            if (result is null)
                return Results.NotFound();
            
            return Results.Ok(result);
        });

        app.MapPost("/flights", async (FlightService service, CreateFlightRequest request) =>
        {
            var command = new CreateFlightCommand
            {
                From = request.From,
                To = request.To
            };
            
            var result = await service.CreateFlight(command);

            var response = new CreateFlightResponse
            {
                FlightId = result.FlightId,
                From = result.From,
                To = result.To
            };
            
            return Results.Created($"/flights/{response.FlightId}", response);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        app.MapPut("/flights/{id:int}", async (int id, UpdateFlightRequest request, 
            IValidator<UpdateFlightRequest> validator, FlightService service) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var command = new UpdateFlightCommand
            {
                FlightId = id,
                From = request.From,
                To = request.To
            };

            var result = await service.UpdateFlight(command);

            if (result is null)
                return Results.NotFound();

            return Results.Ok(result);
            
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        app.MapDelete("/flights/{id:int}", async (int id, FlightService service) =>
        {
            await service.DeleteFlight(id);

            return Results.NoContent();
            
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
        
        app.MapDelete("/flights/clear", async (AppDbContext dbContext) => // фича для тестов только, хардкод
        {
            await dbContext.Flights.ExecuteDeleteAsync();

            return Results.Ok("Flights cleared");
        });
    }
}