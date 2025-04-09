using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _userService;

    public UsersController(ILogger<UsersController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest model)
    {
        var user = await _userService.GetAsync(model.Email);
        if (user != null)
        {
            return BadRequest("User already exists.");
        }
        
        user = await _userService.CreateAsync(model);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginUserRequest model)
    {
        var user = await _userService.GetAsync(model.Email);
        if (user == null)
        {
            return Unauthorized();
        }
        
        return Ok(user);
    }

    [HttpPost("{id:guid}/subscriptions")]
    public async Task<ActionResult<SubscriptionResponse>> CreateSubscription(Guid id, [FromBody] CreateSubscriptionRequest model)
    {
        var user = await _userService.GetAsync(id);
        if (user == null)
        {
            return BadRequest("User with specified id not found.");
        }

        if (!StringComparer.OrdinalIgnoreCase.Equals(user.Email, model.Email))
        {
            return BadRequest("Email mismatch.");
        }

        var response = await _userService.CreateUserSubscription(user.Id, model);
        if (response == null)
        {
            return NotFound("Not found weather for specified location.");
        }
        
        return Ok(response);
    }
    
    [HttpGet("{id:guid}/subscriptions")]
    public async Task<ActionResult<IEnumerable<SubscriptionResponse>>> GetSubscriptions(Guid id)
    {
        var user = await _userService.GetAsync(id);
        if (user == null)
        {
            return BadRequest("User with specified id not found");
        }

        var subs = await _userService.GetUserSubscriptions(id);
        
        return Ok(subs);
    }
    
    [HttpDelete("{id:guid}/subscriptions/{subId:guid}")]
    public async Task<ActionResult> DeleteUserSubscription(Guid id, Guid subId)
    {
        _logger.LogInformation("Deleting subscription {subId} for user {id}", subId, id);
        
        await _userService.DeleteUserSubscriptions(id, subId);
        
        return NoContent();
    }
}