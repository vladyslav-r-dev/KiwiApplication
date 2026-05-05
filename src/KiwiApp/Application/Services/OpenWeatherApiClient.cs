using System.Net;
using System.Text.Json;

namespace KiwiApp.Application.Services;

public class OpenWeatherApiClient(
    HttpClient httpClient,
    IConfiguration configuration)
{
    private const string WeatherUrl =
        "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
    
    public async Task<object?> GetWeather(string city)
    {
        var accessKey = configuration["OpenWeather:AccessKey"];

        if (string.IsNullOrWhiteSpace(accessKey))
            throw new InvalidOperationException("OpenWeather access key is missing.");

        if (string.IsNullOrWhiteSpace(city))
            return null;

        var url = string.Format(
            WeatherUrl,
            Uri.EscapeDataString(city),
            accessKey);

        var response = await httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();

        var weatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (weatherResponse?.Main is null)
            return null;

        return new
        {
            city = weatherResponse.Name ?? city,
            temp = weatherResponse.Main.Temp,
            description = weatherResponse.Weather?.FirstOrDefault()?.Description
        };
    }
}