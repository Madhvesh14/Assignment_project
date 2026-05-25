using Xunit;
using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Data;
using EventBookingAPI.Repositories;
using EventBookingAPI.Models;

namespace EventBookingAPI.Tests.Repositories;

public class AuthRepositoryTests
{
    [Fact]
    public async Task AddUser_AddsUser()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        var repository =
            new AuthRepository(context);

        var user = new User
        {
            FullName = "Test",
            EmailId = "test@gmail.com",
            PasswordHash = "hashed",
            RoleId = 2
        };

        await repository.AddUserAsync(user);

        await repository.SaveChangesAsync();

        Assert.Equal(1, context.Users.Count());
    }
}