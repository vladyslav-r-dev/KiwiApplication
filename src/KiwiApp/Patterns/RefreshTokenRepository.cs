using KiwiApp.Data;
using KiwiApp.DTOs;
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

    public Task Revoke(RefreshToken refreshToken)
    {
        refreshToken.IsRevoked = true;
        return Task.CompletedTask;
    }
}