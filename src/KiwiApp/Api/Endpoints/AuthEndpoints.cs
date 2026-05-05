using KiwiApp.Api.Contracts.Auth;
using KiwiApp.Api.Validator;
using KiwiApp.Application.Services;

namespace KiwiApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/auth/me", (HttpContext context, 
            AuthService authService) =>
        {
            var result = authService.GetMe(context.User);

            return Results.Ok(result);
        }).RequireAuthorization();

        app.MapPost("/register", async (
            RegisterUserDto request,
            RegistrationValidator validator,
            AuthService authService) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            try
            {
                var result = await authService.Register(new RegisterCommand
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password,
                    Passport = request.Passport
                });

                return Results.Created($"/users/{result.UserId}", result.Email);
            }
            catch (InvalidOperationException exception)
            {
                return Results.BadRequest(exception.Message);
            }
        });

        app.MapPost("/auth/login", async (
            LoginUserDto request,
            LoginValidator validator,
            AuthService authService) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }

            var result = await authService.Login(new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            });

            return Results.Ok(result);
        });

        app.MapPost("/auth/logout", async (
            RefreshRequest request,
            AuthService authService) =>
        {
            await authService.Logout(new RefreshCommand
            {
                RefreshToken = request.RefreshToken
            });

            return Results.Ok();
        });

        app.MapPost("/auth/refresh", async (
            RefreshRequest request,
            AuthService authService) =>
        {
            var result = await authService.Refresh(new RefreshCommand 
            {
                RefreshToken = request.RefreshToken 
            });

            return Results.Ok(result);
        });
    }
}