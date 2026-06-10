using EventBookingAPI.DTOs.Authentication;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventBookingAPI.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    private readonly IConfiguration _configuration;

    // Logger instance
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _logger = logger;
    }

    // REGISTER USER

    public async Task<string> RegisterAsync(RegisterDTO dto)
    {
        try
        {
            _logger.LogInformation("Registration attempt for Email {Email}.", dto.EmailId);

            var existingUser = await _authRepository.GetUserByEmailAsync(dto.EmailId);

            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed. User with Email {Email} already exists.", dto.EmailId);

                return "User already exists";
            }

            var user = new User
            {
                FullName = dto.FullName,
                EmailId = dto.EmailId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            await _authRepository.AddUserAsync(user);

            await _authRepository.SaveChangesAsync();

            _logger.LogInformation("User {Email} registered successfully.", dto.EmailId);

            return "User registered successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering user {Email}.", dto.EmailId);

            throw;
        }
    }

    // LOGIN USER
    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
    {
        try
        {
            _logger.LogInformation("Login attempt for Email {Email}.", dto.EmailId);

            var user = await _authRepository.GetUserByEmailAsync(dto.EmailId);

            if (user == null)
            {
                _logger.LogWarning("Login failed. User {Email} not found.", dto.EmailId);

                return null;
            }

            bool isPasswordValid =BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed. Invalid password for {Email}.", dto.EmailId);

                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                new Claim(ClaimTypes.Name, user.FullName),

                new Claim(ClaimTypes.Email, user.EmailId),

                new Claim(ClaimTypes.Role, user.Role!.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!) );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer:
                        _configuration["Jwt:Issuer"],

                    audience:
                        _configuration["Jwt:Audience"],

                    claims:
                        claims,

                    expires:
                        DateTime.UtcNow.AddHours(2),

                    signingCredentials:
                        creds
                );

            _logger.LogInformation("User {Email} logged in successfully.", dto.EmailId);

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),

                Message = "Login successful",

                FullName = user.FullName,

                Email = user.EmailId,

                Role = user.Role!.Name
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging in user {Email}.", dto.EmailId);

            throw;
        }
    }
}