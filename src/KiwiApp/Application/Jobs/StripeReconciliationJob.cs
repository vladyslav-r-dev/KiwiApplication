using Hangfire;
using KiwiApp.Application.Services;
using Stripe.Checkout;

namespace KiwiApp.Application.Jobs;

public class StripeReconciliationJob(
    BookingService bookingService,
    StripeCheckoutService stripeCheckoutService,
    IBackgroundJobClient backgroundJobClient,
    ILogger<StripeReconciliationJob> logger)
{
    public async Task ExecuteAsync()
    {
        var bookings = await bookingService.GetPendingBookingsWithStripeSession();

        foreach (var booking in bookings)
        {
            if (string.IsNullOrWhiteSpace(booking.StripeCheckoutSessionId))
            {
                continue;
            }

            Session session;

            try
            {
                session = await stripeCheckoutService.RetrieveCheckoutSessionAsync(
                    booking.StripeCheckoutSessionId
                );
            }
            catch (Exception e)
            {
                logger.LogError(
                    e,
                    "Failed to retrieve Stripe session {SessionId} for booking {BookingId}",
                    booking.StripeCheckoutSessionId,
                    booking.BookingId
                );

                continue;
            }

            if (session.PaymentStatus == "paid")
            {
                if (!await bookingService.MarkAsPaid(booking.BookingId))
                    continue;

                backgroundJobClient.Enqueue<BookingEmailJob>(
                    job => job.SendBookingConfirmationAsync(booking.BookingId)
                );

                logger.LogInformation(
                    "Booking {BookingId} was marked as paid by Stripe reconciliation",
                    booking.BookingId
                );
            }
            else
            {
                logger.LogInformation(
                    "Booking {BookingId} is still not paid. Stripe session status: {Status}, payment status: {PaymentStatus}",
                    booking.BookingId,
                    session.Status,
                    session.PaymentStatus
                );
            }
        }
    }
}