using KiwiApp.Api.Contracts.Auth;
using KiwiApp.Api.Validator;
using KiwiApp.Application.Services;

namespace KiwiApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/auth/me", async (
        HttpContext context,
        AuthService authService) =>
        {
        var result = await authService.GetMe(context.User);

        return Results.Ok(result);
        }).RequireAuthorization();

        app.MapPut("/auth/me", async (
            UpdateUserDto request,
            AuthService authService,
            HttpContext context) =>
        {
            var result = await authService.UpdateMe(context.User, new UpdateUserCommand
            {
                Name = request.Name,
                LastName = request.LastName,
            });

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

        app.MapPost("/auth/login",
            async (
                LoginUserDto request,
                LoginValidator validator,
                AuthService authService,
                HttpContext context) =>
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

                context.Response.Cookies.Append(
                    "refreshToken",
                    result.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });

                return Results.Ok(new
                {
                    accessToken = result.AccessToken
                });
            });

        app.MapPost("/auth/logout", async (
            RefreshRequest request,
            AuthService authService, HttpContext context) =>
        {
            var refreshToken = context.Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                 return Results.Unauthorized();
            }

            await authService.Logout(new RefreshCommand
            {
                RefreshToken = refreshToken
            });

            context.Response.Cookies.Delete("refreshToken");

            return Results.Ok();
            });

        app.MapPost("/auth/refresh",
            async (
                RefreshRequest request,
                AuthService authService,
                HttpContext context) =>
            {
                var refreshToken = context.Request.Cookies["refreshToken"];

                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Results.Unauthorized();
                }

                var result = await authService.Refresh(new RefreshCommand
                {
                    RefreshToken = refreshToken
                });

                return Results.Ok(new
                {
                    accessToken = result.AccessToken
                });
            });
        
            app.MapPost("/auth/google", async (
            GoogleUserDto request,
            AuthService authService,
            HttpContext context) =>
        {
            var result = await authService.LoginWithGoogle(new GoogleLoginCommand
            {
                IdToken = request.IdToken
            });

            context.Response.Cookies.Append(
                "refreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return Results.Ok(new
            {
                accessToken = result.AccessToken
         });
    });

        app.MapGet("/admin/users", async (AuthService authService) =>
        {
            var users = await authService.GetAllUsers();
            return Results.Ok(users);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}