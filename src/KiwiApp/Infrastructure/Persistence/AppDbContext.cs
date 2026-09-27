using KiwiApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Flight> Flights { get; set; }

    public DbSet<Passenger> Passengers { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Flight)
            .WithMany(f => f.Bookings)
            .HasForeignKey(b => b.FlightId);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);

        base.OnModelCreating(modelBuilder);
    }
}