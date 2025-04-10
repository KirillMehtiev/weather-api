using System.Reflection;
using Refit;
using WebAPI.Middlewares;
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
        var appSettings = builder.Configuration.GetSection("ApplicationSettings");
        
        // Add options
        builder.Services.Configure<OpenWeatherMapOptions>(weatherConfig);
        builder.Services.Configure<ApplicationSettingsOptions>(appSettings);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        builder.Services.AddMemoryCache();
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
        
        app.UseGlobalExceptionHandler();
        app.UseHttpsRedirection();
        // app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}