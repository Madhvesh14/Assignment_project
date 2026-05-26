using EventBookingAPI.Models;

namespace EventBookingAPI.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByEmailAsync(string email);

    Task AddUserAsync(User user);

    Task SaveChangesAsync();
}