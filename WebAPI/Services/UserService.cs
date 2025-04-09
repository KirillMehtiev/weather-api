using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Providers;
using WebAPI.Providers.Models;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IWeatherProvider _weatherProvider;

    public UserService(IUserRepository userRepository, IWeatherProvider weatherProvider)
    {
        _userRepository = userRepository;
        _weatherProvider = weatherProvider;
    }
    
    public Task<User> GetAsync(Guid id)
    {
        var user = _userRepository.GetAsync(id);

        return user;
    }

    public Task<User> GetAsync(string email)
    {
        var user = _userRepository.GetAsync(email);

        return user;
    }

    public Task<User> CreateAsync(CreateUserRequest createUserRequest)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = createUserRequest.Email,
            Subscriptions = new List<Subscription>()
        };
        
        return _userRepository.CreateAsync(user);
    }

    public async Task<IEnumerable<SubscriptionResponse>> GetUserSubscriptions(Guid userId)
    {
        var subs = await _userRepository.GetUserSubscriptions(userId);
        var tasks = new Dictionary<Subscription, Task<WeatherResponse>>();
        var existingSubscriptions = subs.ToList();
        var response = new List<SubscriptionResponse>();

        foreach (var sub in existingSubscriptions)
        {
            tasks[sub] = _weatherProvider.GetWeatherDataAsync(sub.City, sub.Country, sub.ZipCode);
        }

        await Task.WhenAll(tasks.Values);

        foreach (var task in tasks)
        {
            var value = await task.Value;
            response.Add(new SubscriptionResponse(task.Key, value));
        }
        
        return response;
    }
    
    public async Task<SubscriptionResponse> CreateUserSubscription(Guid userId, CreateSubscriptionRequest request)
    {
        var country = ISO3166.Country.List.FirstOrDefault(c => StringComparer.OrdinalIgnoreCase.Equals(c.Name, request.Country));
        if (country == null)
        {
            return null;
        }

        var weatherData = await _weatherProvider.GetWeatherDataAsync(request.City, country.TwoLetterCode, request.ZipCode);
        if (weatherData == null)
        {
            return null;
        }
        
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            Country = request.Country,
            City = request.City,
            ZipCode = request.ZipCode,
            Latitude = weatherData.Coord?.Lat, // might be need for future adjustments
            Longitude = weatherData.Coord?.Lon
        };

        await _userRepository.CreateUserSubscription(userId, subscription);

        var response = new SubscriptionResponse(subscription, weatherData);
        
        return response;
    }

    public Task DeleteUserSubscriptions(Guid userId, Guid subId)
    {
        return _userRepository.DeleteUserSubscriptions(userId, subId);
    }
}