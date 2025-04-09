using Refit;
using WebAPI.Providers.Models;

namespace WebAPI.Providers.Clients;

public interface IOpenWeatherMapApi
{
    [Get("/data/2.5/weather")]
    Task<WeatherResponse> GetCurrentWeatherAsync(
        [Query] string q,
        [Query] string appid,
        [Query] string units);
}