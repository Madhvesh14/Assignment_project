using EventBookingAPI.DTOs.Authentication;
using EventBookingAPI.Models;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventBookingAPI.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IAuthRepository authRepository,
        IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(RegisterDTO dto)
    {
        var existingUser = await _authRepository
            .GetUserByEmailAsync(dto.EmailId);

        if (existingUser != null)
        {
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

        return "User registered successfully";
    }

    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
    {
        var user = await _authRepository
            .GetUserByEmailAsync(dto.EmailId);

        if (user == null)
        {
            return null;
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.EmailId),
            new Claim(ClaimTypes.Role, user.Role!.Name)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new AuthResponseDTO
        {
            Token = new JwtSecurityTokenHandler()
            .WriteToken(token),

            Message = "Login successful",

            FullName = user.FullName,

            Email = user.EmailId,

            Role = user.Role!.Name
        };
    }
}