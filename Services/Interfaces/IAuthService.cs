using EventBookingAPI.DTOs.Authentication;

namespace EventBookingAPI.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDTO dto);

    Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);
}