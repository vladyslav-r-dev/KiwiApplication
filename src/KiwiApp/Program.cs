using FluentValidation;
using KiwiApp.Data;
using KiwiApp.Endpoints;
using KiwiApp.Patterns;
using KiwiApp.Validator;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookingRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateFlightRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegistrationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<ICheckUserData, AuthRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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

app.Run();