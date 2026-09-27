using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetToken(string refreshToken)
    {
        return await db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);
    }

    public Task<RefreshToken> AddToken(string refreshToken, int userId)
    {
        var newToken = new RefreshToken
        {
            Id = new Random().Next(),
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