using System.IdentityModel.Tokens.Jwt;
using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Patterns;

public class RefreshTokenRepository(AppDbContext db) : IRefreshToken
{
    public async Task<RefreshToken?> GetToken(string refreshToken)
    {
        return await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);
    }

    public Task<RefreshToken> AddToken(string refreshToken, Guid userId)
    {
        var newToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            IsRevoked = false,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };
        
        db.RefreshTokens.Add(newToken);
        return Task.FromResult(newToken);
    }

    public Task Revoke(RefreshToken refreshToken)
    {
        refreshToken.IsRevoked = true;
        return Task.CompletedTask;
    }
}