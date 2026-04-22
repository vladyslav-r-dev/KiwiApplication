using FluentValidation;
using KiwiApp.Contracts;
using KiwiApp.Data;
using KiwiApp.Models;
using KiwiApp.Patterns;

namespace KiwiApp.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingsEndpoints(this WebApplication app)
    {
        app.MapGet("/bookings", async (IBookingRepository bookingRepository) =>
        {
            return await bookingRepository.GetAllBookings();
        });
        
        app.MapGet("/bookings/{id}", async (Guid id, IBookingRepository bookingRepository) =>
        {
            var bookingId = await bookingRepository.GetBooking(id);
            if (bookingId is null)
            {
                return Results.NotFound();
            }
            
            return Results.Ok(bookingId);
        });

        app.MapPost("/bookings", async (CreateBookingRequest request, IValidator<CreateBookingRequest> validator, AppDbContext db, 
            IBookingRepository bookingRepository, IFlightRepository flightRepository, IUnitOfWork unitOfWork) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
            
            var flight = await flightRepository.GetFlight(request.FlightId);
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

            await bookingRepository.AddBooking(booking);
    
            var response = new CreateBookingResponse
            {
                BookingId = booking.BookingId,
                Status = booking.Status,
                Price = booking.Price
            };

            await unitOfWork.SaveChangesAsync();
            return Results.Created($"/bookings/{booking.BookingId}", response);
        });

        app.MapPut("/bookings/{id}", async (Guid id, UpdateBookingRequest request, IValidator<UpdateBookingRequest> validator, 
            IBookingRepository bookingRepository, IUnitOfWork unitOfWork) =>
        {
            var booking = await bookingRepository.GetBooking(id);
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
            
            await unitOfWork.SaveChangesAsync();
            return Results.Ok(booking);
        });

        app.MapDelete("/bookings/{id}", async (Guid id,
            IBookingRepository bookingRepository, IUnitOfWork unitOfWork) =>
        {
            var booking = await bookingRepository.GetBooking(id);
    
            if (booking is null)
            {
                return Results.NotFound();
            }
    
            await bookingRepository.RemoveBooking(booking);
            await unitOfWork.SaveChangesAsync();
    
            return Results.NoContent();
        });
    }
   
}