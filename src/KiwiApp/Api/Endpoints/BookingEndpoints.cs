using FluentValidation;
using KiwiApp.Api.Contracts;
using KiwiApp.Application.Services;
using KiwiApp.Application.UseCases.Bookings;
using KiwiApp.Application.UseCases.Bookings.Create;
using KiwiApp.Application.UseCases.Bookings.Create.Update;
using KiwiApp.Application.UseCases.Bookings.Update;

namespace KiwiApp.Api.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingsEndpoints(this WebApplication app)
    {
        app.MapGet("/bookings", async (BookingService bookingService) =>
        {
            return await bookingService.GetAllBookings();
        });
        
        app.MapGet("/bookings/{id}", async (int id, BookingService bookingService) =>
        {
            var result = await bookingService.GetBookingById(id);
            
            return Results.Ok(result);
        });

        app.MapPost("/bookings", async (CreateBookingRequest request, IValidator<CreateBookingRequest> validator,
            BookingService service) =>
        {
            var validation = await validator.ValidateAsync(request);
            
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var command = new CreateBookingCommand
            {
                FlightId = request.FlightId,
                Passengers = request.Passengers,
                Email = request.Email
            };
            
            var result = await service.Execute(command);
            
            if (result is null)
                return Results.NotFound();
            
            var response = new CreateBookingResponse
            {
                BookingId = result.BookingId,
                Status = result.Status,
                Price = result.Price
            };

            return Results.Created($"/bookings/{response.BookingId}", response);
        });

        app.MapPut("/bookings/{id}", async (int id, UpdateBookingRequest request, IValidator<UpdateBookingRequest> validator,
            BookingService service) =>
        {
            var validation = await validator.ValidateAsync(request);
            
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var command = new UpdateBookingCommand
            {
                Email = request.Email,
                Passengers = request.Passengers,
            };

            var result = await service.UpdateBooking(id, command);

            return Results.Ok(result);
        });

        app.MapDelete("/bookings/{id}", async (int id, 
            BookingService service) =>
        {
            await service.DeleteBooking(id);
            
            return Results.NoContent();
        });
    }
   
}