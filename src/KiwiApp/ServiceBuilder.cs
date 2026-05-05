using KiwiApp.Application.Interfaces;
using KiwiApp.Application.Services;
using KiwiApp.Application.UseCases;
using KiwiApp.Application.UseCases.Bookings;
using KiwiApp.Application.UseCases.Bookings.Create;
using KiwiApp.Application.UseCases.Flights;
using KiwiApp.Infrastructure.Repositories;

namespace KiwiApp;

public static class ServiceBuilder
{
    public static void ServiceCollection(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<AviationstackImportService>();
        builder.Services.AddHttpClient<AviationstackImportService>();
        builder.Services.AddScoped<OpenWeatherApiClient>();
        builder.Services.AddHttpClient<OpenWeatherApiClient>();
        builder.Services.AddScoped<BookingService>();
        builder.Services.AddScoped<FlightService>();
        builder.Services.AddScoped<AuthService>();
    }
    
    public static void AddRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IFlightRepository, FlightRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<ICheckUserData, AuthRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IRefreshToken, RefreshTokenRepository>();
    }
}