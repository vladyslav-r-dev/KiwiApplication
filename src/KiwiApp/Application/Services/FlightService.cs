using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases.Flights.Create;
using KiwiApp.Application.UseCases.Flights.Get;
using KiwiApp.Application.UseCases.Flights.Search;
using KiwiApp.Application.UseCases.Flights.Update;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Services;

public class FlightService(IGenericRepository<Flight> repository, 
    IUnitOfWork unitOfWork, ILogger<FlightService> logger, 
    OpenWeatherApiClient  openWeatherApiClient, 
    IFlightRepository flightRepository,
    AviationstackImportService importService)
{
    public async Task<CreateFlightResult> CreateFlight(CreateFlightCommand command)
    {
        var flight = Flight.CreateFlight(command.From, command.To);
        
        await repository.Add(flight);
        
        await unitOfWork.SaveChangesAsync();

        return new CreateFlightResult
        {
            FlightId = flight.FlightId,
            From = flight.From,
            To = flight.To
        };
    }
    
    public async Task<UpdateFlightResult?> UpdateFlight(UpdateFlightCommand command)
    {
        var flight = await repository.GetById(command.FlightId);

        if (flight is null)
        {
            return null;
        }
        
        flight.UpdateFlight(command.From, command.To);

        await unitOfWork.SaveChangesAsync();
        
        return new UpdateFlightResult
        {
            FlightId = flight.FlightId,
            From = flight.From,
            To = flight.To
        };
    }
    
    public async Task<IEnumerable<Flight>> GetAllFlights()
    {
        return await repository.GetAll();
    }

    public async Task<GetFlightResult> GetAllFlightById(int id)
    {
        var flight = await repository.GetById(id);

        if (flight is null)
        {
            logger.LogWarning("Flight with id {FlightId} was not found", id);

            throw new KeyNotFoundException($"Flight with id {id} was not found");
        }

        return new GetFlightResult
        {
            FlightId = flight.FlightId,
            From = flight.From,
            To = flight.To
        };
    }
    
    public async Task<bool> DeleteFlight(int id)
    {
        var flightId = await repository.GetById(id);

        if (flightId is null)
        {
            logger.LogWarning("Flight with id {FlightId} was not found", id);

            throw new KeyNotFoundException($"Flight with id {id} was not found");
        }

        await repository.Remove(flightId);
        
        await unitOfWork.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<SearchFlightResult?> SearchFlight(string from, string to)
    {
        var search = await flightRepository.GetFlightFromTo(from, to);
        
        var weatherFrom = await openWeatherApiClient.GetWeather(from);
        
        var weatherTo = await openWeatherApiClient.GetWeather(to);

        return new SearchFlightResult
        {
            Flight = search,
            WeatherFrom = weatherFrom,
            WeatherTo = weatherTo
        };
    }
    
    public async Task ImportFlights()
    {
        await importService.ImportFlightsAsync();
    }
}