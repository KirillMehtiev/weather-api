using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WebAPI.Exceptions;
using WebAPI.Providers.Clients;
using WebAPI.Providers.Models;
using WebAPI.SettingOptions;

namespace WebAPI.Providers;

public class WeatherProvider : IWeatherProvider
{
    private readonly IOpenWeatherMapApi _client;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<WeatherProvider> _logger;
    private readonly OpenWeatherMapOptions _settings;
    
    public WeatherProvider(IOptions<OpenWeatherMapOptions> options, IOpenWeatherMapApi client,
        IMemoryCache memoryCache, ILogger<WeatherProvider> logger)
    {
        _client = client;
        _memoryCache = memoryCache;
        _logger = logger;
        _settings = options.Value;
    }

    public async Task<WeatherResponse> GetWeatherDataAsync(string city, string countryCode, string zipCode = null)
    {
        string cacheKey = $"{city},{countryCode}";

        if (_memoryCache.TryGetValue(cacheKey, out WeatherResponse response))
        {
            return response;
        }

        try
        {
            response = await _client.GetCurrentWeatherAsync($"{city},{countryCode}", _settings.ApiKey, _settings.Units);
            _memoryCache.Set(cacheKey, response, TimeSpan.FromMinutes(_settings.CacheTimeMinutes));
        }
        catch (Refit.ApiException apiEx) when (apiEx.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogInformation($"Not found: {city},{countryCode}");
        }
        catch (Exception e)
        {
            throw new ThirdPartyException("Unexpected error when accessing third-party API", e);
        }
        
        return response;
    }
    
}