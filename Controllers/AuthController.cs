using EventBookingAPI.DTOs.Authentication;
using EventBookingAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Logger instance
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    
    // REGISTER USER
    // POST: api/auth/register
    

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        try
        {
            _logger.LogInformation("Registration request received for Email: {Email}", dto.EmailId);

            var result = await _authService.RegisterAsync(dto);

            if (result == "User already exists")
            {
                _logger.LogWarning("Registration failed. User already exists with Email: {Email}", dto.EmailId);
                 return BadRequest(result);
            }

            _logger.LogInformation("User registered successfully with Email: {Email}", dto.EmailId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering user with Email: {Email}", dto.EmailId);

            return StatusCode(500, "An internal server error occurred.");
        }
    }

   
    // LOGIN USER
    // POST: api/auth/login
    

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        try
        {
            _logger.LogInformation("Login request received for Email: {Email}", dto.EmailId);
             
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                _logger.LogWarning("Invalid login attempt for Email: {Email}", dto.EmailId);

                return Unauthorized("Invalid email or password");
            }

            _logger.LogInformation("User logged in successfully with Email: {Email}", dto.EmailId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"An error occurred while logging in user with Email: {Email}", dto.EmailId);

            return StatusCode(500,"An internal server error occurred.");
        }
    }
}