namespace WebAPI.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    
    public string Country { get; set; }

    public string City { get; set; }

    public string ZipCode { get; set; }
    
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}