using System.Text.Json;
using KiwiApp.Data;
using KiwiApp.Models;
using KiwiApp.Patterns;

namespace KiwiApp.Services;

public class AviationstackImportService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    IUnitOfWork unitOfWork,
    AppDbContext dbContext)
{
    public async Task ImportFlightsAsync()
    {
        var accessKey = configuration["Aviationstack:AccessKey"];
        var httpClient = httpClientFactory.CreateClient();
        var url = $"http://api.aviationstack.com/v1/flights?access_key={accessKey}&limit=25";
        var response = await httpClient.GetAsync(url);
        var json =  await response.Content.ReadAsStringAsync();
     
        var jsonDocument = JsonDocument.Parse(json);
        var root = jsonDocument.RootElement;
        var data = root.GetProperty("data");

        foreach (var item in data.EnumerateArray())
        {
            var departureAirport = item.GetProperty("departure")
                .GetProperty("airport")
                .GetString();

            var arrivalAirport = item.GetProperty("arrival")
                .GetProperty("airport")
                .GetString();
                
            if (string.IsNullOrWhiteSpace(departureAirport) ||
                string.IsNullOrWhiteSpace(arrivalAirport))
            {
                continue;
            }

            var flight = new Flight
            {
                FlightId = Guid.NewGuid(),
                From = departureAirport,
                To = arrivalAirport
            };
                
            dbContext.Flights.Add(flight);
        }
        
        await unitOfWork.SaveChangesAsync();
    }
}