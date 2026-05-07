using System.Text.Json.Serialization;

namespace KiwiApp.Api.Contracts;

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
}

public class AviationstackAirportDto
{
    [JsonPropertyName("airport")]
    public string? Airport { get; set; }
}