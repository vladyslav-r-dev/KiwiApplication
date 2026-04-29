using FluentValidation;
using KiwiApp.Application.UseCases.Flights;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Patterns;
using KiwiApp.Services;

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
            var flightId = await flightRepository.GetFlight(id);

            return Results.Ok(flightId);
        });
        
        app.MapGet("/flights/import/api", async (AviationstackImportService importService) =>
        {
            await importService.ImportFlightsAsync();
            return Results.Ok("Flights imported");
        });

        app.MapGet("/search/flight", async (IFlightRepository flightRepository, string? from, string? to, 
            OpenWeatherService openWeatherService) =>
        {
            var search = await flightRepository.GetFlightFromTo(from, to);
            var weatherFrom = await openWeatherService.GetWeather(from);
            var weatherTo = await openWeatherService.GetWeather(to);
            return Results.Ok(new
            {
                flights = search,
                to = weatherFrom,
                from = weatherTo
            });
        });

        app.MapPost("/flights", async (CreateFlightUseCase useCase, CreateFlightRequest request) =>
        {
            var command = new CreateFlightCommand
            {
                From = request.From,
                To = request.To
            };
            
            var result = await useCase.Execute(command);

            var response = new CreateFlightResponse
            {
                FlightId = result.FlightId,
                From = result.From,
                To = result.To
            };
            
            return Results.Created($"/flights/{response.FlightId}", response);
        });

        app.MapPut("/flights/{id}", async (Guid id, UpdateFlightRequest request, 
            IValidator<UpdateFlightRequest> validator,IFlightRepository flightRepository, IUnitOfWork unitOfWork) =>
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
            await unitOfWork.SaveChangesAsync();
    
            return Results.Ok(flight);
        });

        app.MapDelete("/flights/{id}", async (Guid id,
            IFlightRepository flightRepository, IUnitOfWork unitOfWork) =>
        {
            var flightId = await flightRepository.GetFlight(id);
            if (flightId is null)
            {
                return Results.NotFound();
            }
            
            await flightRepository.RemoveFlight(flightId);
            await unitOfWork.SaveChangesAsync();
    
            return Results.NoContent();
        });
        
        app.MapDelete("/flights/clear", async (AppDbContext dbContext, IUnitOfWork unitOfWork) =>
        {
            dbContext.Flights.RemoveRange(dbContext.Flights);
            await unitOfWork.SaveChangesAsync();

            return Results.Ok("Flights cleared");
        });
    }
}