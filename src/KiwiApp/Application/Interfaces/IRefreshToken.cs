using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IRefreshToken
{
    Task<RefreshToken?> GetToken(string refreshToken);
    
    Task<RefreshToken> AddToken(string refreshToken, int userId);
    
    Task Revoke(RefreshToken refreshToken);
}