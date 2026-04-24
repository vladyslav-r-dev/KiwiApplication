using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KiwiApp.Models;
using Microsoft.IdentityModel.Tokens;

namespace KiwiApp.Services;

public class TokenService
{
    public string CreateAccesToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };
            
        var key = new SymmetricSecurityKey("9fH3kL8xQ2vPz7A1mN4sD6wR0yT5uB8cE1gJ9hK2L4M6nP8rS0vX3Z5"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );
            
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}