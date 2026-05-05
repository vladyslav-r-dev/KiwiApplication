using System.Text.Json;
using KiwiApp.Api.Contracts;
using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;

namespace KiwiApp.Application.Services;

public class AviationstackImportService(
    HttpClient httpClient,
    IConfiguration configuration,
    IUnitOfWork unitOfWork,
    AppDbContext dbContext)
{
    private const string AviaUrl = "http://api.aviationstack.com/v1/flights";
    public async Task ImportFlightsAsync()
    {
        var accessKey = configuration["Aviationstack:AccessKey"];
        
        var url = $"{AviaUrl}?access_key={accessKey}&limit=25";
        
        var response = await httpClient.GetAsync(url);
        
        var json =  await response.Content.ReadAsStringAsync();
     
       var jsonResponse =  JsonSerializer.Deserialize<AviationstackResponse>(json);
       
       if (jsonResponse is null)
       {
           throw new Exception("Не удалось прочитать ответ от Aviationstack");
       }

        foreach (var item in jsonResponse.Data)
        {
            var departureAirport = item.Departure.Airport;

            var arrivalAirport = item.Arrival.Airport;
                
            if (string.IsNullOrWhiteSpace(departureAirport) ||
                string.IsNullOrWhiteSpace(arrivalAirport))
            {
                continue;
            }

            var flight = new Flight
            {
                From = departureAirport,
                To = arrivalAirport
            };
                
            dbContext.Flights.Add(flight);
        }
        
        await unitOfWork.SaveChangesAsync();
    }
}