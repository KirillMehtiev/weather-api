using System.Collections.Concurrent;
using WebAPI.Entities;

namespace WebAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ConcurrentBag<User> _users;
    
    public UserRepository()
    {
        _users = new ConcurrentBag<User>();
    }

    public Task<User> GetAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        return Task.FromResult(user);
    }

    public Task<User> GetAsync(string email)
    {
        var user = _users.FirstOrDefault(u => StringComparer.OrdinalIgnoreCase.Equals(u.Email, email));

        return Task.FromResult(user);
    }

    public Task<User> CreateAsync(User user)
    {
        _users.Add(user);

        return Task.FromResult(user);
    }

    public Task<IEnumerable<Subscription>> GetUserSubscriptions(Guid userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        var subs = user?.Subscriptions?.Take(100);
        
        return Task.FromResult(subs);
    }

    public Task<Subscription> CreateUserSubscription(Guid userId, Subscription subscription)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            user.Subscriptions ??= new List<Subscription>();
            user.Subscriptions.Add(subscription);
        }

        return Task.FromResult(subscription);
    }

    public Task DeleteUserSubscriptions(Guid userId, Guid subId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user?.Subscriptions != null)
        {
            var sub = user.Subscriptions.FirstOrDefault(s => s.Id == subId);
            if (sub != null)
            {
                user.Subscriptions.Remove(sub);
            }
        }

        return Task.CompletedTask;
    }
}