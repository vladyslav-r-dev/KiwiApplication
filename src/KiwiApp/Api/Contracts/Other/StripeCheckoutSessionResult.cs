namespace KiwiApp.Api.Contracts.Other;

public class StripeCheckoutSessionResult
{
    public string SessionId { get; set; }
    public string CheckoutUrl { get; set; }
}