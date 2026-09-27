using System.Text.Json.Serialization;

namespace KiwiApp.Application.Services.Models;

public class AviationstackResponse
{
    [JsonPropertyName("data")]
    public List<AviationstackFlightDto> Data { get; set; } = [];
}

public class AviationstackFlightDto
{
    [JsonPropertyName("departure")]
    public AviationstackAirportDto Departure { get; set; } = new();

    [JsonPropertyName("arrival")]
    public AviationstackAirportDto Arrival { get; set; } = new();

    [JsonPropertyName("airline")]
    public AviationstackAirlineDto Airline { get; set; } = new();

    [JsonPropertyName("flight")]
    public AviationstackFlightInfoDto Flight { get; set; } = new();

    [JsonPropertyName("flight_status")]
    public string? FlightStatus { get; set; }
}

public class AviationstackAirportDto
{
    [JsonPropertyName("airport")]
    public string? Airport { get; set; }

    [JsonPropertyName("iata")]
    public string? Iata { get; set; }
    [JsonPropertyName("scheduled")]
    public string? Scheduled { get; set; }
}

public class AviationstackAirlineDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class AviationstackFlightInfoDto
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }
}