using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<string>> Create([FromBody] CreateUserRequest model)
    {
        return Ok("created");
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest model)
    {
        return Ok("success");
    }

    [HttpPost("{id:guid}/subscriptions")]
    public async Task<ActionResult<string>> CreateSubscription(Guid id, [FromBody] CreateSubscriptionRequest model)
    {
        return Ok("created");
    }
    
    [HttpGet("{id:guid}/subscriptions")]
    public async Task<ActionResult<string>> GetSubscriptions(Guid id)
    {
        return Ok("list");
    }
    
    [HttpDelete("{id:guid}/subscriptions/{subId}")]
    public async Task<ActionResult<string>> GetSubscriptions(Guid id, Guid subId)
    {
        return Ok("list");
    }
    

}