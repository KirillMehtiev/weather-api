using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

/// <summary>
/// API for managing users and their subscriptions
/// </summary>
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

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="model.email">Email is required for user creation.</param>
    /// <returns>The created user.</returns>
    /// <response code="200">User successfully created.</response>
    /// <response code="400">User already exists or request is invalid.</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="model.email">Email is required for user login.</param>
    /// <returns>The logged-in user data.</returns>
    /// <response code="200">User successfully logged in.</response>
    /// <response code="401">Unauthorized - user not found</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginUserRequest model)
    {
        var user = await _userService.GetAsync(model.Email);
        if (user == null)
        {
            return Unauthorized();
        }
        
        return Ok(user);
    }

    /// <summary>
    /// Creates a weather subscription for the specified user.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="model.email">Email is required.</param>
    /// <param name="model.country">Country is required.</param>
    /// <param name="model.city">City is required.</param>
    /// <param name="model.zipCode">City is optional.</param>
    /// <returns>The created subscription.</returns>
    /// <response code="200">Subscription created.</response>
    /// <response code="400">User not found or email mismatch.</response>
    /// <response code="404">Weather data not found for location.</response>
    [HttpPost("{id:guid}/subscriptions")]
    [ProducesResponseType(typeof(SubscriptionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    
    /// <summary>
    /// Gets all subscriptions of a user.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>List of subscriptions.</returns>
    /// <response code="200">Returns the list of subscriptions.</response>
    /// <response code="400">User not found.</response>
    [HttpGet("{id:guid}/subscriptions")]
    [ProducesResponseType(typeof(IEnumerable<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    
    /// <summary>
    /// Deletes a user's subscription.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="subId">Subscription ID.</param>
    /// <response code="204">Subscription deleted.</response>
    [HttpDelete("{id:guid}/subscriptions/{subId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUserSubscription(Guid id, Guid subId)
    {
        _logger.LogInformation("Deleting subscription {subId} for user {id}", subId, id);
        
        await _userService.DeleteUserSubscriptions(id, subId);
        
        return NoContent();
    }
}