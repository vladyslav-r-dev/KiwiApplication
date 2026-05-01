using System.Text.Json;

namespace KiwiApp.Infrastructure.Services;

public class OpenWeatherService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
{
    
    public async Task<object> GetWeather(string city)
    {
        var accessKey = configuration["OpenWeather:AccessKey"];
        var httpClient = httpClientFactory.CreateClient();

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={accessKey}&units=metric";

        var response = await httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        using var jsonDocument = JsonDocument.Parse(json);
        var root = jsonDocument.RootElement;

        var temperature = root.GetProperty("main")
            .GetProperty("temp")
            .GetDouble();

        var name = root.GetProperty("name").GetString();

        return new
        {
            city = name,
            temp = temperature
        };
    }
}