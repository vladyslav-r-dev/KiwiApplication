using KiwiApp.Patterns;
using KiwiApp.Services;

namespace KiwiApp.Application.UseCases.Flights;

public class SearchFlightUseCase(IFlightRepository  flightRepository, OpenWeatherService  openWeatherService)
{
    public async Task<SearchFlightResult> SearchFlight(string from, string to)
    {
        var search = await flightRepository.GetFlightFromTo(from, to);
        var weatherFrom = await openWeatherService.GetWeather(from);
        var weatherTo = await openWeatherService.GetWeather(to);

        return new SearchFlightResult
        {
            Flights = search,
            WeatherFrom = weatherFrom,
            WeatherTo = weatherTo
        };
    }
}