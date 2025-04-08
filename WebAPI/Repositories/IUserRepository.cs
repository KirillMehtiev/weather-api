using WebAPI.Entities;

namespace WebAPI.Repositories;

public interface IUserRepository
{
    Task<User> GetAsync(Guid id);
    Task<User> GetAsync(string email);
    Task<User> CreateAsync(User user);
    
    Task<IEnumerable<Subscription>> GetUserSubscriptions(Guid userId);
    Task<Subscription> CreateUserSubscription(Guid userId, Subscription subscription);
    Task DeleteUserSubscriptions(Guid userId, Guid subId);
}