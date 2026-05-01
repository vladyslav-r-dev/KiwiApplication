using KiwiApp.Infrastructure.Services;

namespace KiwiApp.Application.UseCases.Flights;

public class ImportFlightsUseCase(AviationstackImportService importService)
{
    public async Task ImportFlights()
    {
        await importService.ImportFlightsAsync();
    }
}