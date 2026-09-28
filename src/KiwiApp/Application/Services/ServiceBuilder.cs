using Hangfire;
using Hangfire.MemoryStorage;
using KiwiApp.Application.Interfaces;
using KiwiApp.Application.Jobs;
using KiwiApp.Infrastructure.Repositories;

namespace KiwiApp.Application.Services;

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
        builder.Services.AddScoped<PdfService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<StripeWebhookService>();
        builder.Services.AddScoped<StripeCheckoutService>();
        builder.Services.AddHangfire(config =>
        {
            config.UseMemoryStorage();
        });

        builder.Services.AddHangfireServer();
        builder.Services.AddScoped<StripeReconciliationJob>();
        builder.Services.AddScoped<GoogleAuthService>();
        builder.Services.AddScoped<CacheService>();
        builder.Services.AddDistributedMemoryCache();
    }

    public static void AddRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IFlightRepository, FlightRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IUserRepository, AuthRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<BookingEmailJob>();
    }
}