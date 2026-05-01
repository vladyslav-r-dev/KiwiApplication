using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.UseCases.Flights.Create;

public class CreateFlightUseCase(IFlightRepository flightRepository, IUnitOfWork unitOfWork)
{
    public async Task<CreateFlightResult> Execute(CreateFlightCommand command)
    {
        var flight = Flight.CreateFlight(command.From, command.To);
        
        await flightRepository.AddFlight(flight);
        await unitOfWork.SaveChangesAsync();

        return new CreateFlightResult
        {
            FlightId = flight.FlightId,
            From = flight.From,
            To = flight.To
        };
    }
}