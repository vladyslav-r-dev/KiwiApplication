using KiwiApp.Models;
using KiwiApp.Patterns;

namespace KiwiApp.Application.UseCases.Flights;

public class GetAllFlightsUseCase(IFlightRepository flightRepository)
{
    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        return await flightRepository.GetAllFlights();
    }

    public async Task<GetFlightResult?> GetAllFlightsById(Guid id)
    {
        var flight = await flightRepository.GetFlight(id);

        if (flight is null)
            return null;

        return new GetFlightResult
        {
            FlightId = flight.FlightId,
            From = flight.From,
            To = flight.To
        };
    }
}