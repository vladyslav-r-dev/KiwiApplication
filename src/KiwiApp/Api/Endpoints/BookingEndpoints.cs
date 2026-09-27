using KiwiApp.Api.Mapping;
using System.Security.Claims;
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
        app.MapGet("/bookings", async (
            BookingService bookingService,
            ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null ||
                !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.Unauthorized();
            }

            var result = await bookingService.GetBookingByUserId(userId);

            return Results.Ok(result);
        })
        .RequireAuthorization();


        app.MapGet("/bookings/{id}", async (
            int id,
            BookingService bookingService,
            ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null ||
                !int.TryParse(userIdClaim.Value, out var currentUserId))
            {
                return Results.Unauthorized();
            }

            var booking = await bookingService.GetBookingById(id);

            if (booking is null)
                return Results.NotFound();

            if (booking.UserId != currentUserId &&
                !user.IsInRole("Admin"))
            {
                return Results.Forbid();
            }

            return Results.Ok(booking);
        })
        .RequireAuthorization();


        app.MapPost("/bookings", async (
            CreateBookingRequest request,
            IValidator<CreateBookingRequest> validator,
            BookingService service,
            StripeCheckoutService stripeCheckoutService,
            ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null ||
                !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.Unauthorized();
            }

            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var passengerCommands = request.Passengers
                .Select(p => new CreatePassengerCommand
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    SelectedSeatNumber = p.SelectedSeatNumber
                })
                .ToList();

            var command = new CreateBookingCommand
            {
                FlightId = request.FlightId,
                Passengers = passengerCommands,
                Email = request.Email,
                UserId = userId
            };

            var result = await service.CreateBooking(command);

            if (result is null)
                return Results.NotFound();

            var checkoutSession =
                await stripeCheckoutService.CreateCheckoutSessionAsync(
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

            return Results.Created(
                $"/bookings/{response.BookingId}",
                response
            );
        })
        .RequireAuthorization();


        app.MapPost("/stripe/webhook", async (
            StripeWebhookService stripeWebhookService,
            HttpRequest request) =>
        {
            return await stripeWebhookService.HandleWebhookAsync(request);
        });


        app.MapPut("/bookings/{id}", async (
            int id,
            UpdateBookingRequest request,
            IValidator<UpdateBookingRequest> validator,
            BookingService service,
            ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null ||
                !int.TryParse(userIdClaim.Value, out var currentUserId))
            {
                return Results.Unauthorized();
            }

            var booking = await service.GetBookingById(id);

            if (booking is null)
                return Results.NotFound();

            if (booking.UserId != currentUserId &&
                !user.IsInRole("Admin"))
            {
                return Results.Forbid();
            }

            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var command = new UpdateBookingCommand
            {
                Email = request.Email,
                Passengers = request.Passengers?.Select(new UpdateBookingRequestMapper().ToCommand).ToList()!
            };

            var result = await service.UpdateBooking(id, command);

            return Results.Ok(result);
        })
        .RequireAuthorization();


        app.MapDelete("/bookings/{id}", async (
            int id,
            BookingService service,
            ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null ||
                !int.TryParse(userIdClaim.Value, out var currentUserId))
            {
                return Results.Unauthorized();
            }

            var booking = await service.GetBookingById(id);

            if (booking is null)
                return Results.NotFound();

            if (booking.UserId != currentUserId &&
                !user.IsInRole("Admin"))
            {
                return Results.Forbid();
            }

            await service.DeleteBooking(id);

            return Results.NoContent();
        })
        .RequireAuthorization();

        app.MapGet("/admin/bookings", async (
            BookingService service) =>
        {
            var bookings = await service.GetAllBookings();
            return Results.Ok(bookings);
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}