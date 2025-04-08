using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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

    public Task<IEnumerable<Subscription>> GetUserSubscriptions(Guid userId)
    {
        var subs = _userRepository.GetUserSubscriptions(userId);

        return subs;
    }
    
    public Task<Subscription> CreateUserSubscription(Guid userId, CreateSubscriptionRequest subscription)
    {
        var sub = new Subscription
        {
            Id = Guid.NewGuid(),
            Country = subscription.Country,
            City = subscription.City,
            ZipCode = subscription.ZipCode
        };
        
        return _userRepository.CreateUserSubscription(userId, sub);
    }

    public Task DeleteUserSubscriptions(Guid userId, Guid subId)
    {
        return _userRepository.DeleteUserSubscriptions(userId, subId);
    }
}