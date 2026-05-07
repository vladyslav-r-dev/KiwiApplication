public class OpenWeatherResponse
{
    public MainWeather? Main { get; set; }
    
    public List<WeatherInfo>? Weather { get; set; }
    
    public string? Name { get; set; }
}

public class MainWeather
{
    public double Temp { get; set; }
}

public class WeatherInfo
{
    public string? Description { get; set; }
}