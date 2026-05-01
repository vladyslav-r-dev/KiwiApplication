using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Flights.Search;

public class SearchFlightResult
{
    public IEnumerable<Flight> Flights { get; set; }

    public object? WeatherFrom { get; set; }

    public object? WeatherTo { get; set; }
}