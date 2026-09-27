namespace KiwiApp.Application.UseCases.Payments;

public class StripeCheckoutSessionResult
{
    public string SessionId { get; set; }
    public string CheckoutUrl { get; set; }
}