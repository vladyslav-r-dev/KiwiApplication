using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KiwiApp.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace KiwiApp.Application.Services;

public class TokenService(IConfiguration configuration)
{
    private const int TokenLifetimeMinutes = 30;
    public string CreateAccessToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Token:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(TokenLifetimeMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}