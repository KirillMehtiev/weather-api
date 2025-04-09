using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class CreateSubscriptionRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Country { get; set; }
    
    [Required]
    public string City { get; set; }
    
    public string ZipCode { get; set; }
}