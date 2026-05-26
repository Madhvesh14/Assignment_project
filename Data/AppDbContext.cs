using Microsoft.EntityFrameworkCore;
using EventBookingAPI.Models;

namespace EventBookingAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Event> Events => Set<Event>();

    public DbSet<Booking> Bookings => Set<Booking>();


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        
        // EVENTDATE UTC FIX

        modelBuilder.Entity<Event>()
            .Property(e => e.EventDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(
                    v,
                    DateTimeKind.Utc));

        
        // CREATEDAT UTC FIX

        modelBuilder.Entity<Event>()
            .Property(e => e.CreatedAt)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(
                    v,
                    DateTimeKind.Utc));

        
        // BOOKINGDATE UTC FIX

        modelBuilder.Entity<Booking>()
            .Property(b => b.BookingDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(
                    v,
                    DateTimeKind.Utc));

        
        // USER → ROLE

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        
        // BOOKING → USER

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        
        // BOOKING → EVENT

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Event)
            .WithMany(e => e.Bookings)
            .HasForeignKey(b => b.EventId);

        base.OnModelCreating(modelBuilder);
    }
}