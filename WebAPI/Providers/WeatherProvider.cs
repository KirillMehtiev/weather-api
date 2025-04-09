using Microsoft.Extensions.Options;
using WebAPI.Providers.Clients;
using WebAPI.Providers.Models;
using WebAPI.SettingOptions;

namespace WebAPI.Providers;

public class WeatherProvider : IWeatherProvider
{
    private readonly IOpenWeatherMapApi _client;
    private readonly OpenWeatherMapOptions _settings;
    
    public WeatherProvider(IOptions<OpenWeatherMapOptions> options, IOpenWeatherMapApi client)
    {
        _client = client;
        _settings = options.Value;
    }

    public async Task<WeatherResponse> GetWeatherDataAsync(string city, string countryCode, string zipCode = null)
    {
        WeatherResponse response = null;
        try
        {
            response = await _client.GetCurrentWeatherAsync($"{city},{countryCode}", _settings.ApiKey, _settings.Units);
        }
        catch (Exception e)
        {
            // todo: add error handling 404 etc.
            throw;
        }
        
        return response;
    }
    
}