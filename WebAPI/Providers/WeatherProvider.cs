using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebAPI.Providers.Clients;
using WebAPI.Providers.Models;
using WebAPI.SettingOptions;

namespace WebAPI.Providers;

public class WeatherProvider : IWeatherProvider
{
    private readonly IOpenWeatherMapApi _client;
    private readonly IMemoryCache _memoryCache;
    private readonly OpenWeatherMapOptions _settings;
    
    public WeatherProvider(IOptions<OpenWeatherMapOptions> options, IOpenWeatherMapApi client, IMemoryCache memoryCache)
    {
        _client = client;
        _memoryCache = memoryCache;
        _settings = options.Value;
    }

    public async Task<WeatherResponse> GetWeatherDataAsync(string city, string countryCode, string zipCode = null)
    {
        WeatherResponse response = null;
        string cacheKey = $"{city},{countryCode}";

        if (_memoryCache.TryGetValue(cacheKey, out response))
        {
            return response;
        }
        
        try
        {
            response = await _client.GetCurrentWeatherAsync($"{city},{countryCode}", _settings.ApiKey, _settings.Units);
            _memoryCache.Set(cacheKey, response, TimeSpan.FromMinutes(_settings.CacheTimeMinutes));
        }
        catch (Exception e)
        {
            // todo: add error handling 404 etc.
            throw;
        }
        
        return response;
    }
    
}