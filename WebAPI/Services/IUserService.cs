using WebAPI.Entities;
using WebAPI.Models;

namespace WebAPI.Services;

public interface IUserService
{
    Task<UserResponse> GetAsync(Guid id);
    Task<UserResponse> GetAsync(string email);
    Task<UserResponse> CreateAsync(CreateUserRequest createUserRequest);
    
    Task<IEnumerable<SubscriptionResponse>> GetUserSubscriptions(Guid userId);
    Task<SubscriptionResponse> CreateUserSubscription(Guid userId, CreateSubscriptionRequest subscription);
    Task DeleteUserSubscriptions(Guid userId, Guid subId);
}