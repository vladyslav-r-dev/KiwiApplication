using KiwiApp.DTOs;
using KiwiApp.Models;

namespace KiwiApp.Patterns;

public interface IRefreshToken
{
    Task<RefreshToken?> GetToken(string refreshToken);
    Task Revoke(RefreshToken refreshTokenoken);
}