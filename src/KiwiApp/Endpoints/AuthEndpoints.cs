using System.Security.Claims;
using KiwiApp.DTOs;
using KiwiApp.Models;
using KiwiApp.Patterns;
using KiwiApp.Services;
using KiwiApp.Validator;

namespace KiwiApp.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/auth/me", (HttpContext context) =>
        {
            var user = context.User;

            var userId = user.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var email = user.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return Results.Ok(new
            {
                userId,
                email
            });
        }).RequireAuthorization();
        
        app.MapPost("/register", async (RegisterUserDto request, IUnitOfWork unitOfWork, 
            ICheckUserData checkUserData, RegistrationValidator validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
            
            var existingUser = await checkUserData.GetUserByEmail(request.Email);
            if (existingUser is not null)
            {
                return Results.BadRequest("User already exists");
            }
            
            var newUser = new UserEntity
            {
                Id =  Guid.NewGuid(),
                FirstName = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Passport = request.Passport,
            };
            
            await checkUserData.AddUser(newUser);
            await unitOfWork.SaveChangesAsync();

            return Results.Created($"/users/{newUser.Id}", newUser.Email);
        });
        
        app.MapPost("/auth/login", async (LoginUserDto request,
            ICheckUserData checkUserData, LoginValidator validator, 
            TokenService tokenService, IRefreshToken refreshTokenRepository) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.BadRequest(validation.Errors);
            }
            
            var email = request.Email;
            var password = request.Password;
            var existingUser = await checkUserData.GetUserByEmail(email);
            
            if (existingUser is null)
            {
                return Results.BadRequest("Email doesn't exist"); 
            }
            if (password != existingUser.Password)
            {
                return Results.BadRequest("Wrong password"); 
            }
            
            var accessToken = tokenService.CreateAccesToken(existingUser);
            var refreshToken = Guid.NewGuid().ToString();
            
            await refreshTokenRepository.AddToken(refreshToken, existingUser.Id);

            return Results.Ok(new { accessToken, refreshToken });
        });
        
        app.MapPost("/auth/logout", async (RefreshRequest request, IUnitOfWork unitOfWork, 
            IRefreshToken refreshTokenRepository) =>
        {
            var refreshToken = await refreshTokenRepository.GetToken(request.RefreshToken);

            if (refreshToken is null) return Results.Ok();
            
            await refreshTokenRepository.Revoke(refreshToken);
            await unitOfWork.SaveChangesAsync();
            
            return Results.Ok();
        });

        app.MapPost("/auth/refresh", async (RefreshRequest request,
            IRefreshToken refreshTokenRepository, ICheckUserData checkUserData, TokenService tokenService) =>
        {
            var refreshToken = await refreshTokenRepository.GetToken(request.RefreshToken);
            if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return Results.Unauthorized();
            }

            var existingUser = await checkUserData.GetUserById(refreshToken.UserId);
            var accessToken = tokenService.CreateAccesToken(existingUser);

            return Results.Ok(new { accessToken });
        });
    }
}