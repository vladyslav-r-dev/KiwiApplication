using System.Text;
using FluentValidation;
using KiwiApp.Data;
using KiwiApp.Endpoints;
using KiwiApp.Patterns;
using KiwiApp.Services;
using KiwiApp.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
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
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookingRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateFlightRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegistrationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<ICheckUserData, AuthRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRefreshToken, RefreshTokenRepository>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AviationstackImportService>();

var key = "9fH3kL8xQ2vPz7A1mN4sD6wR0yT5uB8cE1gJ9hK2L4M6nP8rS0vX3Z5";

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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        context.Response.StatusCode = 500;

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Internal server error"
        });
    });
});

app.UseHttpsRedirection();
app.MapBookingsEndpoints();
app.MapFlightEndpoints();
app.MapAuthEndpoints();
app.UseAuthentication();
app.UseAuthorization();

app.Run();