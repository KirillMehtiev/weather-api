using WebAPI.Providers.Models;

namespace WebAPI.Providers;

public interface IWeatherProvider
{
    Task<WeatherResponse> GetWeatherDataAsync(string city, string countryCode, string zipCode = null);
}