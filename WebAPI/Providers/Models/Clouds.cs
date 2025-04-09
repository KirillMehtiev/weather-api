using System.Text.Json.Serialization;

namespace WebAPI.Providers.Models;

public class Clouds
{
    [JsonPropertyName("all")]
    public int? All { get; set; }
}