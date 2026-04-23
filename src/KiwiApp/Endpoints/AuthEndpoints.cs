using KiwiApp.DTOs;
using KiwiApp.Models;
using KiwiApp.Patterns;
using KiwiApp.Validator;

namespace KiwiApp.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
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
            return Results.Ok();
        });
    }
}