using WebAPI.Entities;

namespace WebAPI.Models;

public class UserResponse
{
    public UserResponse(User user)
    {
        Id = user.Id;
        Email = user.Email;
    }
    
    public Guid Id { get; set; }
    
    public string Email { get; set; }
}