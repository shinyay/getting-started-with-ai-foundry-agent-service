using Microsoft.AspNetCore.Mvc;
using ReadingExperience.Core.DTOs;
using ReadingExperience.Core.Interfaces;

namespace ReadingExperience.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponseDto>> Register([FromBody] UserRegistrationDto request)
    {
        try
        {
            var result = await _userService.RegisterAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] UserLoginDto request)
    {
        try
        {
            var result = await _userService.LoginAsync(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }
}