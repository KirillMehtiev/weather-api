using Refit;
using WebAPI.Providers;
using WebAPI.Providers.Clients;
using WebAPI.Repositories;
using WebAPI.Services;
using WebAPI.SettingOptions;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var weatherConfig = builder.Configuration.GetSection("OpenWeatherMap");
        
        // Add options
        builder.Services.Configure<OpenWeatherMapOptions>(weatherConfig);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddRefitClient<IOpenWeatherMapApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(weatherConfig["BaseUrl"]));

        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IWeatherProvider, WeatherProvider>();
       
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        // app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}