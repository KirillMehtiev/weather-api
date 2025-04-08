namespace WebAPI.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }

    public IList<Subscription> Subscriptions { get; set; }
}