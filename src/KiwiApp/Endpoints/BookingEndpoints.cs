using FluentValidation;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;

namespace KiwiApp.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingsEndpoints(this WebApplication app, AppData appData)
    {
        app.MapGet("/bookings", () => appData.Bookings);
        
        app.MapGet("/bookings/{id}", (Guid id) =>
        {
            var bookingId = appData.Bookings.FirstOrDefault(x => x.BookingId == id);
            if (bookingId is null)
            {
                return Results.NotFound();
            }
    
            return Results.Ok(bookingId);
        });

        app.MapPost("/bookings", async (CreateBookingRequest request, IValidator<CreateBookingRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
            
            var flight = appData.Flights.FirstOrDefault(x => x.FlightId == request.FlightId);
            if (flight is null)
                return Results.NotFound();
            

            
            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                FlightId = request.FlightId,
                Passengers = request.Passengers,
                Email = request.Email,
                Price = 100,
                Status = BookingStatus.Pending
            };

            appData.Bookings.Add(booking);
    
            var response = new CreateBookingResponse
            {
                BookingId = booking.BookingId,
                Status = booking.Status,
                Price = booking.Price
            };
    
            return Results.Created($"/bookings/{booking.BookingId}", response);
        });

        app.MapPut("/bookings/{id}", async (Guid id, UpdateBookingRequest request, IValidator<UpdateBookingRequest> validator) =>
        {
            var booking = appData.Bookings.FirstOrDefault(x => x.BookingId == id);
            if (booking is null)
            {
                return Results.NotFound();
            }
            
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
    
            booking.Passengers = request.Passengers;
            booking.Email = request.Email;
            booking.Status = BookingStatus.Updated;
    
            return Results.Ok(booking);
        });

        app.MapDelete("/bookings/{id}", (Guid id) =>
        {
            var booking = appData.Bookings.FirstOrDefault(x => x.BookingId == id);
    
            if (booking is null)
            {
                return Results.NotFound();
            }
    
            appData.Bookings.Remove(booking);
    
            return Results.NoContent();
        });
    }
   
}