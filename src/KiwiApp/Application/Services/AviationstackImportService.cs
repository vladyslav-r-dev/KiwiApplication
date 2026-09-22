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

            var departureIata = item.Departure.Iata;
            var arrivalIata = item.Arrival.Iata;
            var airline = item.Airline.Name;
            var flightNumber = item.Flight.Number;
            var departureTime = item.Departure.Scheduled;
            var arrivalTime = item.Arrival.Scheduled;
            var status = item.FlightStatus;
                
            if (string.IsNullOrWhiteSpace(departureAirport) ||
                string.IsNullOrWhiteSpace(arrivalAirport))
            {
                continue;
            }

            var flight = new Flight
            {
                From = departureAirport,
                To = arrivalAirport,
                FromIata = departureIata,
                ToIata = arrivalIata,
                Airline = airline,
                FlightNumber = flightNumber,
                DepartureTime = DateTime.TryParse(departureTime, out var depTime) ? depTime : null,
                ArrivalTime = DateTime.TryParse(arrivalTime, out var arrTime) ? arrTime : null,
                Status = status,
                Price = Random.Shared.Next(100, 1000)
            };

            var seatLetters = new[] { 'A', 'B', 'C', 'D', 'E', 'F' };

            for (var row = 1; row <= 10; row++)
            {
                foreach (var letter in seatLetters)
                {
                    var seat = new Seat
                    {
                        SeatNumber = $"{row}{letter}",
                        IsOccupied = false
                    };

                    flight.Seats.Add(seat);
                }
            }

            var occupiedSeats = flight.Seats
                .OrderBy(_ => Random.Shared.Next())
                .Take(50);

            foreach (var seat in occupiedSeats)
            {
                seat.IsOccupied = true;
            }
                
            dbContext.Flights.Add(flight);
        }
        
        await unitOfWork.SaveChangesAsync();
    }
}