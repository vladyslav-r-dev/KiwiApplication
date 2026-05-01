using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IRefreshToken
{
    Task<RefreshToken?> GetToken(string refreshToken);
    Task<RefreshToken> AddToken(string refreshToken, Guid userId);
    Task Revoke(RefreshToken refreshToken);
}