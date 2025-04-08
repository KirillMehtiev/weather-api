using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}