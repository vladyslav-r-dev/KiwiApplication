using KiwiApp.Application.UseCases.Shared;

namespace KiwiApp.Application.UseCases.Flights.Search;

public class SearchFlightResult
{
    public IEnumerable<FlightResult> Flight { get; set; }

    public object? WeatherFrom { get; set; }

    public object? WeatherTo { get; set; }
}