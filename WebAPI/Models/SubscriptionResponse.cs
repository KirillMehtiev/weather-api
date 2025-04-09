using System.Globalization;
using WebAPI.Entities;
using WebAPI.Providers.Models;

namespace WebAPI.Models;

public class SubscriptionResponse
{
    public SubscriptionResponse(Subscription subscription, WeatherResponse weatherData)
    {
        Id = subscription.Id;
        City = subscription.City;
        Country = subscription.Country;
        ZipCode = subscription.ZipCode;

        if (weatherData?.Main != null)
        {
            Temperature = weatherData.Main.Temp;
            TempMin = weatherData.Main.TempMin;
            TempMax = weatherData.Main.TempMax;
            Humidity = weatherData.Main.Humidity;
            Pressure = weatherData.Main.Pressure;
            Description = weatherData.Weather.FirstOrDefault()?.Description;
            WindSpeed = weatherData.Wind?.Speed;
            Cloudiness = weatherData.Clouds.All switch
            {
                <= 10 => "Clear",
                <= 40 => "Partly Cloudy",
                <= 70 => "Mostly Cloudy",
                <= 100 => "Overcast",
                _ => "Unknown"
            };

            if (weatherData.Sys != null)
            {
                // better to do formating at UI (to handle such case as 15:00 vs 3:00 pm)
                Sunrise = DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunrise + weatherData.Timezone)
                    .ToString("t", CultureInfo.InvariantCulture);
                Sunset = DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunset + weatherData.Timezone)
                    .ToString("t", CultureInfo.InvariantCulture);
            }
        }
    }
    
    public Guid Id { get; set; }
    
    public string Country { get; set; }

    public string City { get; set; }

    public string ZipCode { get; set; }
    
    public string Description { get; set; }
    
    public float? Temperature { get; set; }
    
    public float? TempMin { get; set; }
    
    public float? TempMax { get; set; }
    
    public int? Pressure { get; set; }
    
    public int? Humidity { get; set; }
    
    public float? WindSpeed { get; set; }
    
    public string Cloudiness { get; set; }
    
    public string Sunrise { get; set; }
    
    public string Sunset { get; set; }
}