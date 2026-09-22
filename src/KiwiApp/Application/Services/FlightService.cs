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
    AviationstackImportService importService, CacheService cacheService)
{
    public async Task<CreateFlightResult> CreateFlight(CreateFlightCommand command)
    {
        var flight = Flight.CreateFlight(command.From, command.To);
        
        await repository.Add(flight);
        
        await unitOfWork.SaveChangesAsync();

        await cacheService.RemoveAsync("flights:all");
        
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
        
        await cacheService.RemoveAsync("flights:all");
        await cacheService.RemoveAsync($"flights:id:{command.FlightId}");
        
        return new UpdateFlightResult
        {
            FlightId = flight.FlightId,
            From = flight.From,
            To = flight.To
        };
    }

    public async Task<IEnumerable<Flight>> GetFilteredFlights(
        string? from,
        string? to,
        string? status,
        decimal? minPrice,
        decimal? maxPrice,
        string? sortBy,
        string? airline,
        string? departureDate)
    {
        return await flightRepository.GetFilteredFlights(from, to, status, minPrice, maxPrice, sortBy, airline, departureDate);
    }
    
    public async Task<IEnumerable<Flight>> GetAllFlights()
    {   
        const string cacheKey = "flights:all";
        
        var cachedFlights = await cacheService.GetAsync<Flight[]>(cacheKey);
        
        if (cachedFlights is not null)
        {
            return cachedFlights;
        }
        
        var flights = (await repository.GetAll()).ToArray();

        await cacheService.SetAsync(cacheKey, flights, TimeSpan.FromMinutes(5));
        
        return flights;
    }

    public async Task<GetFlightResult> GetAllFlightById(int id)
{
    var flight = await flightRepository.GetFlightById(id);

    if (flight is null)
    {
        logger.LogWarning(
            "Flight with id {FlightId} was not found",
            id
        );

        throw new KeyNotFoundException(
            $"Flight with id {id} was not found"
        );
    }

    return new GetFlightResult
    {
        FlightId = flight.FlightId,
        From = flight.From,
        To = flight.To,

        Seats = flight.Seats
            .Select(seat => new SeatResult
            {
                SeatNumber = seat.SeatNumber,
                IsOccupied = seat.IsOccupied
            })
            .ToList()
    };
}
    
    public async Task<bool> DeleteFlight(int id)
    {
        var flight = await flightRepository.GetFlightById(id);

        if (flight is null)
        {
            logger.LogWarning("Flight with id {FlightId} was not found", id);

            throw new KeyNotFoundException($"Flight with id {id} was not found");
        }

        await repository.Remove(flight);
        
        await unitOfWork.SaveChangesAsync();
        
        await cacheService.RemoveAsync("flights:all");
        await cacheService.RemoveAsync($"flights:id:{flight.FlightId}");
        
        return true;
    }
    
    public async Task<SearchFlightResult?> SearchFlight(string? from, string? to)
    {
        var search = await flightRepository.GetFlightFromTo(from, to);
        
        var weatherFrom = from is not null ? await openWeatherApiClient.GetWeather(from) : null;
        
        var weatherTo = to is not null ? await openWeatherApiClient.GetWeather(to) : null;

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
        
        await cacheService.RemoveAsync("flights:all");
    }
}