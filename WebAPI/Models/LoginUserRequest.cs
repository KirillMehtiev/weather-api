using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class LoginUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}