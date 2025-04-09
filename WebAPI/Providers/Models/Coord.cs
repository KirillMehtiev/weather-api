using System.Text.Json.Serialization;

namespace WebAPI.Providers.Models;

public class Coord
{
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}