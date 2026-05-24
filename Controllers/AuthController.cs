using EventBookingAPI.DTOs.Authentication;
using EventBookingAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace EventBookingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    
    // REGISTER USER
    // POST: api/auth/register

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (result == "User already exists")
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    
    // LOGIN USER
    // POST: api/auth/login

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
        {
            return Unauthorized("Invalid email or password");
        }

        return Ok(result);
    }
}