using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Data;

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