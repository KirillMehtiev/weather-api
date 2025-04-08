using WebAPI.Entities;
using WebAPI.Models;

namespace WebAPI.Services;

public interface IUserService
{
    Task<User> GetAsync(Guid id);
    Task<User> GetAsync(string email);
    Task<User> CreateAsync(CreateUserRequest createUserRequest);
    
    Task<IEnumerable<Subscription>> GetUserSubscriptions(Guid userId);
    Task<Subscription> CreateUserSubscription(Guid userId, CreateSubscriptionRequest subscription);
    Task DeleteUserSubscriptions(Guid userId, Guid subId);
}