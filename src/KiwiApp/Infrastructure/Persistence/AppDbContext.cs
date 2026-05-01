using KiwiApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet <Booking> Bookings { get; set; } 
    public DbSet <Flight> Flights { get; set; } 
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}