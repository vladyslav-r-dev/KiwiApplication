using FluentValidation;
using KiwiApp.Api.Contracts;
using KiwiApp.Api.Contracts.Booking;
using KiwiApp.Application.Services;
using KiwiApp.Application.UseCases.Bookings.Create;
using KiwiApp.Application.UseCases.Bookings.Update;
using KiwiApp.Domain.Entities;

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
            BookingService service, StripeCheckoutService stripeCheckoutService) =>
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
            
            var checkoutSession = await stripeCheckoutService.CreateCheckoutSessionAsync(
                result.BookingId,
                request.Email,
                result.Price
            );

            await service.SetStripeCheckoutSessionId(
                result.BookingId,
                checkoutSession.SessionId
            );
            
            var response = new CreateBookingResponse
            {
                BookingId = result.BookingId,
                Status = BookingStatus.Pending,
                Price = result.Price,
                CheckoutUrl = checkoutSession.CheckoutUrl
            };

            return Results.Created($"/bookings/{response.BookingId}", response);
        });
        
        app.MapPost("/stripe/webhook", async (StripeWebhookService stripeWebhookService, HttpRequest request) =>
        {
            return await stripeWebhookService.HandleWebhookAsync(request);
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