using FluentValidation;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingsEndpoints(this WebApplication app)
    {
        app.MapGet("/bookings", async (AppDbContext db) =>
        {
            return await db.Bookings.ToListAsync();
        });
        
        app.MapGet("/bookings/{id}", async (Guid id, AppDbContext db) =>
        {
            var bookingId = await db.Bookings.FirstOrDefaultAsync(x => x.BookingId == id);
            if (bookingId is null)
            {
                return Results.NotFound();
            }
            
            return Results.Ok(bookingId);
        });

        app.MapPost("/bookings", async (CreateBookingRequest request, IValidator<CreateBookingRequest> validator, AppDbContext db) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
            
            var flight = await db.Flights.FirstOrDefaultAsync(x => x.FlightId == request.FlightId);
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

            db.Bookings.Add(booking);
    
            var response = new CreateBookingResponse
            {
                BookingId = booking.BookingId,
                Status = booking.Status,
                Price = booking.Price
            };
            
            await db.SaveChangesAsync();
            return Results.Created($"/bookings/{booking.BookingId}", response);
        });

        app.MapPut("/bookings/{id}", async (Guid id, UpdateBookingRequest request, IValidator<UpdateBookingRequest> validator, AppDbContext db) =>
        {
            var booking = await db.Bookings.FirstOrDefaultAsync(x => x.BookingId == id);
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
            
            await db.SaveChangesAsync();
            return Results.Ok(booking);
        });

        app.MapDelete("/bookings/{id}", async (Guid id,  AppDbContext db) =>
        {
            var booking = await db.Bookings.FirstOrDefaultAsync(x => x.BookingId == id);
    
            if (booking is null)
            {
                return Results.NotFound();
            }
    
            db.Bookings.Remove(booking);
            await db.SaveChangesAsync();
    
            return Results.NoContent();
        });
    }
   
}