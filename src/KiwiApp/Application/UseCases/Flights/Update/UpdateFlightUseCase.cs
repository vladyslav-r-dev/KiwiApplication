using KiwiApp.Application.Interfaces;

namespace KiwiApp.Application.UseCases.Flights.Update;

public class UpdateFlightUseCase(IFlightRepository flightRepository, IUnitOfWork unitOfWork)
{
    public async Task<UpdateFlightResult?> UpdateAsync(UpdateFlightCommand command)
    {
        var flight = await flightRepository.GetFlight(command.FlightId);

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
}