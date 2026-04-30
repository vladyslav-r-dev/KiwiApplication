using KiwiApp.Application.UseCases.Bookings;
using KiwiApp.Application.UseCases.Flights;
using KiwiApp.Application.UseCases.Flights.Update;
using KiwiApp.Patterns;
using KiwiApp.Services;

namespace KiwiApp;

public static class ServiceBuilder
{
    public static void ServiceCollection(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<AviationstackImportService>();
        builder.Services.AddScoped<OpenWeatherService>();
        builder.Services.AddScoped<CreateBookingUseCase>();
        builder.Services.AddScoped<CreateFlightUseCase>();
        builder.Services.AddScoped<GetAllFlightsUseCase>();
        builder.Services.AddScoped<ImportFlightsUseCase>();
        builder.Services.AddScoped<UpdateFlightUseCase>();
        builder.Services.AddScoped<DeleteFlightUseCase>();
    }
    
    public static void AddRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IFlightRepository, FlightRepository>();
        builder.Services.AddScoped<ICheckUserData, AuthRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IRefreshToken, RefreshTokenRepository>();
    }
}