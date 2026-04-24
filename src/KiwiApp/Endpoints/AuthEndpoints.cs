using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KiwiApp.DTOs;
using KiwiApp.Models;
using KiwiApp.Patterns;
using KiwiApp.Validator;
using Microsoft.IdentityModel.Tokens;

namespace KiwiApp.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/auth/me", async (HttpContext context, ICheckUserData checkUserData) =>
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
            ICheckUserData checkUserData, LoginValidator validator) =>
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
            
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new(ClaimTypes.Email, existingUser.Email)
            };
            
            var key = new SymmetricSecurityKey("9fH3kL8xQ2vPz7A1mN4sD6wR0yT5uB8cE1gJ9hK2L4M6nP8rS0vX3Z5"u8.ToArray());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            return Results.Ok(new
            {
                accessToken = tokenString
            });
        });
        
        app.MapPost("/auth/logout", async (RefreshRequest request, UnitOfWork unitOfWork, 
            IRefreshToken refreshTokenRepository) =>
        {
            var refreshToken = await refreshTokenRepository.GetToken(request.RefreshToken);

            if (refreshToken is null) return Results.Ok();
            
            await refreshTokenRepository.Revoke(refreshToken);
            await unitOfWork.SaveChangesAsync();
            
            return Results.Ok();
        });
        
        // app.MapPost("/auth/refresh", async ())
    }
}