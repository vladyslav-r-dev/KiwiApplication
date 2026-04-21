using FluentValidation;
using KiwiApp.Data;
using KiwiApp.Endpoints;
using KiwiApp.Validator;
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


var app = builder.Build();
var data = new AppData();

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
app.MapBookingsEndpoints(data);
app.MapFlightEndpoints(data);

app.Run();