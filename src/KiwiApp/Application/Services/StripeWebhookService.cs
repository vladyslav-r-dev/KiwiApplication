using Hangfire;
using KiwiApp.Application.Jobs;
using Stripe;
using Stripe.Checkout;

namespace KiwiApp.Application.Services;

public class StripeWebhookService(
    IConfiguration configuration, BookingService bookingService,
    ILogger<StripeWebhookService> logger, IBackgroundJobClient backgroundJobClient)
{
    public async Task<IResult> HandleWebhookAsync(HttpRequest request)
    {
        var json = await new StreamReader(request.Body).ReadToEndAsync();

        var endpointSecret = configuration["Stripe:WebhookSecret"];

        try
        {
            var signatureHeader = request.Headers["Stripe-Signature"];

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                signatureHeader,
                endpointSecret
            );

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;

                if (!int.TryParse(session?.ClientReferenceId, out var bookingId))
                {
                    return Results.BadRequest();
                }

                if (await bookingService.MarkAsPaid(bookingId))
                {
                    backgroundJobClient.Enqueue<BookingEmailJob>(
                        job => job.SendBookingConfirmationAsync(bookingId)
                    );
                }
            }

            return Results.Ok();
        }
        catch (StripeException e)
        {
            logger.LogWarning("Stripe webhook error: {0}", e.Message);
            return Results.BadRequest();
        }
        catch (Exception e)
        {
            logger.LogWarning("Webhook error: {0}", e.Message);
            return Results.StatusCode(500);
        }
    }
}