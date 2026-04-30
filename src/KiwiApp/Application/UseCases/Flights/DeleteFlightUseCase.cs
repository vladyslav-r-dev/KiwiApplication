using KiwiApp.Patterns;

namespace KiwiApp.Application.UseCases.Flights;

public class DeleteFlightUseCase(IFlightRepository flightRepository, IUnitOfWork unitOfWork)
{
    public async Task<bool> DeleteFlight(Guid id)
    {
        var flightId = await flightRepository.GetFlight(id);

        if (flightId == null) 
            return false;
        
        await flightRepository.RemoveFlight(flightId);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}