namespace KiwiApp.Contracts;

public class UpdateFlightRequest
{
    public Guid NewFlightId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}