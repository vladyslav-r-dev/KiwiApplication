namespace KiwiApp.Application.UseCases.Flights;

public class CreateFlightCommand
{
    public string From { get; set; }
    public string To { get; set; }
}