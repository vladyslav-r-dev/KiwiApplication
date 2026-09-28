using System.Text;
using FluentValidation;
using Hangfire;
using KiwiApp.Api.Endpoints;
using KiwiApp.Api.ErrorHandling;
using KiwiApp.Api.Validator;
using KiwiApp.Application.Jobs;
using KiwiApp.Application.Services;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Вставь JWT токен"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddProblemDetails();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Flight.Create", policy => policy.RequireRole("Admin"));
});
builder.Services.AddAuthentication();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingRequestValidator>();

ServiceBuilder.AddRepositories(builder);
ServiceBuilder.ServiceCollection(builder);

var key = builder.Configuration["Token:Key"];
if (string.IsNullOrWhiteSpace(key) || Encoding.UTF8.GetByteCount(key) < 32)
{
    throw new InvalidOperationException("Configure Token:Key (at least 32 UTF-8 bytes) using User Secrets or Token__Key.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "https://kiwi-application.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Configure ConnectionStrings:DefaultConnection (ConnectionStrings__DefaultConnection) with a PostgreSQL connection string.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors("Frontend");

app.UseGlobalExceptionHandler();

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<StripeReconciliationJob>(
    "stripe-reconciliation",
    job => job.ExecuteAsync(),
    Cron.MinuteInterval(15)
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapBookingsEndpoints();
app.MapFlightEndpoints();
app.MapAuthEndpoints();

app.Run();