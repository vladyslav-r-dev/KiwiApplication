using KiwiApp.Models;

namespace KiwiApp.Application.UseCases.Flights;

public class SearchFlightResult
{
    public IEnumerable<Flight> Flights { get; set; }

    public object? WeatherFrom { get; set; }

    public object? WeatherTo { get; set; }
}