using FluentValidation;
using KiwiApp.Api.Contracts;
using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases.Flights;
using KiwiApp.Application.UseCases.Flights.Create;
using KiwiApp.Application.UseCases.Flights.Get;
using KiwiApp.Application.UseCases.Flights.Search;
using KiwiApp.Application.UseCases.Flights.Update;
using KiwiApp.Infrastructure.Persistence;

namespace KiwiApp.Api.Endpoints;

public static class FlightEndpoints
{
    public static void MapFlightEndpoints(this WebApplication app)
    {
        app.MapGet("/flights", async (GetAllFlightsUseCase useCase) =>
        {
            var result = await useCase.GetAllFlights();
            return Results.Ok(result);
        });

        app.MapGet("/flights/{id}", async (GetAllFlightsUseCase useCase, Guid id) =>
        {
            var result = await useCase.GetAllFlightsById(id);

            if (result is null)
                return Results.NotFound();
            
            return Results.Ok(result);
        });
        
        app.MapGet("/flights/import/api", async (ImportFlightsUseCase useCase) =>
        {
            await useCase.ImportFlights();
            return Results.Ok("Flights imported");
        });

        app.MapGet("/search/flight", async (SearchFlightUseCase useCase, string from, string to) =>
        {
            var result = await useCase.SearchFlight(from, to);
            return Results.Ok(result);
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
            IValidator<UpdateFlightRequest> validator, UpdateFlightUseCase useCase) =>
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

            var result = await useCase.UpdateAsync(command);

            if (result is null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        app.MapDelete("/flights/{id}", async (Guid id, DeleteFlightUseCase useCase) =>
        {
            var deleted = await useCase.DeleteFlight(id);

            if (!deleted)
                return Results.NotFound();

            return Results.NoContent();
        });
        
        app.MapDelete("/flights/clear", async (AppDbContext dbContext, IUnitOfWork unitOfWork) => // фича для тестов только, хардкод
        {
            dbContext.Flights.RemoveRange(dbContext.Flights);
            await unitOfWork.SaveChangesAsync();

            return Results.Ok("Flights cleared");
        });
    }
}