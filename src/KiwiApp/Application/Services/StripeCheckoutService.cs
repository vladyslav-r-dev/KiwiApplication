using KiwiApp.Application.UseCases.Payments;
using Stripe;
using Stripe.Checkout;

namespace KiwiApp.Application.Services;

public class StripeCheckoutService(IConfiguration configuration)
{
    public async Task<StripeCheckoutSessionResult> CreateCheckoutSessionAsync(
        int bookingId,
        string email,
        decimal price)
    {
        var baseUrl = (configuration["Frontend:BaseUrl"] ?? "http://localhost:4200").TrimEnd('/');

        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{baseUrl}/payment-success",
            CancelUrl = $"{baseUrl}/payment-cancel",

            ClientReferenceId = bookingId.ToString(),

            CustomerEmail = email,

            Metadata = new Dictionary<string, string>
            {
                ["bookingId"] = bookingId.ToString()
            },

            LineItems =
            [
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = (long)Math.Round(price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Flight booking"
                        }
                    },
                    Quantity = 1
                }
            ],

            Mode = "payment"
        };

        var secretKey = configuration["Stripe:SecretKey"];

        var client = new StripeClient(secretKey);

        var stripe = client.V1.Checkout.Sessions;

        Session session = await stripe.CreateAsync(options);

        return new StripeCheckoutSessionResult
        {
            SessionId = session.Id,
            CheckoutUrl = session.Url
        };
    }

    public async Task<Session> RetrieveCheckoutSessionAsync(string sessionId)
    {
        var secretKey = configuration["Stripe:SecretKey"];

        var client = new StripeClient(secretKey);

        var service = client.V1.Checkout.Sessions;

        return await service.GetAsync(sessionId);
    }
}