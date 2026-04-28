using System.Text.Json;
using FluentValidation;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;
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
        
        // app.MapGet("/search/flight/api", async (IHttpClientFactory httpClientFactory, 
        //     IConfiguration configuration, string departureRequest, string arrivalRequest, IFlightRepository flightRepository) =>
        // {
        //     var requestDeparture = departureRequest;
        //     var requestArrival = arrivalRequest;
        //     var accessKey = configuration["Aviationstack:AccessKey"];
        //     var httpClient = httpClientFactory.CreateClient();
        //     var url = $"http://api.aviationstack.com/v1/flights?access_key={accessKey}&limit=25";
        //     var response = await httpClient.GetAsync(url);
        //     var json =  await response.Content.ReadAsStringAsync();
        //     
        //     var jsonDocument = JsonDocument.Parse(json);
        //     var root = jsonDocument.RootElement;
        //     var flights = root.GetProperty("data");
        //
        //     foreach (var flight in flights.EnumerateArray())
        //     {
        //         var departure = flight.GetProperty("departure")
        //             .GetProperty("airport").GetString();
        //         var arrival = flight.GetProperty("arrival").
        //             GetProperty("airport").GetString();
        //         
        //         if (departure == requestDeparture && arrival == requestArrival)
        //         {
        //             return Results.Ok(flight);
        //         }
        //     }
        //     
        //     return Results.NotFound("Flight not found");
        // });
        
        app.MapGet("/flights/import/api", async (AviationstackImportService importService) =>
        {
            await importService.ImportFlightsAsync();
            return Results.Ok("Flights imported");
        });

        app.MapGet("/search/flight", async (IFlightRepository flightRepository, string? from, string? to) =>
        {
            var search = await flightRepository.GetFlightFromTo(from, to);
            return Results.Ok(search);
        });

        app.MapPost("/flights", async (Flight flight, IFlightRepository flightRepository, IUnitOfWork unitOfWork) =>
        {
            flight.FlightId = Guid.NewGuid();
    
            await flightRepository.AddFlight(flight);
            await unitOfWork.SaveChangesAsync();
            return Results.Created($"/flights/{flight.FlightId}", flight);
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