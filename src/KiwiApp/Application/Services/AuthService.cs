using System.Security.Claims;
using KiwiApp.Api.Contracts.Auth;
using KiwiApp.Application.Interfaces;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    ICheckUserData checkUserData,
    IGenericRepository<UserEntity> userRepository,
    IRefreshToken refreshTokenRepository,
    TokenService tokenService,
    ILogger<AuthService> logger)
{
    public MeResult GetMe(ClaimsPrincipal user)
    {
        var userId = user.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var email = user.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        return new MeResult
        {
            UserId = userId,
            Email = email
        };
    }

    public async Task<RegisterResult> Register(RegisterCommand command)
    {
        var existingUser = await checkUserData.GetUserByEmail(command.Email);

        if (existingUser is not null)
        {
            logger.LogWarning("User with email {Email} already exists", command.Email);

            throw new InvalidOperationException("User already exists");
        }

        var newUser = new UserEntity
        {
            FirstName = command.Name,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password,
            Passport = command.Passport,
        };

        await userRepository.Add(newUser);

        await unitOfWork.SaveChangesAsync();

        return new RegisterResult
        {
            UserId = newUser.Id,
            Email = newUser.Email
        };
    }

    public async Task<LoginResult> Login(LoginCommand command)
    {
        var existingUser = await checkUserData.GetUserByEmail(command.Email);

        if (existingUser is null)
        {
            logger.LogWarning("Login failed. Email {Email} was not found", command.Email);

            throw new KeyNotFoundException("Email doesn't exist");
        }

        if (command.Password != existingUser.Password)
        {
            logger.LogWarning("Login failed. Wrong password for email {Email}", command.Email);

            throw new UnauthorizedAccessException("Wrong password");
        }

        var accessToken = tokenService.CreateAccesToken(existingUser);

        var refreshToken = Guid.NewGuid().ToString();

        await refreshTokenRepository.AddToken(refreshToken, existingUser.Id);

        await unitOfWork.SaveChangesAsync();

        return new LoginResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task Logout(RefreshCommand command)
    {
        var refreshToken = await refreshTokenRepository.GetToken(command.RefreshToken);

        if (refreshToken is null)
        {
            logger.LogWarning("Logout requested with non-existing refresh token");

            return;
        }

        await refreshTokenRepository.Revoke(refreshToken);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<RefreshResult> Refresh(RefreshCommand command)
    {
        var refreshToken = await refreshTokenRepository.GetToken(command.RefreshToken);

        if (refreshToken is null)
        {
            logger.LogWarning("Refresh token was not found");

            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        if (refreshToken.IsRevoked)
        {
            logger.LogWarning("Refresh token is revoked");

            throw new UnauthorizedAccessException("Refresh token is revoked");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            logger.LogWarning("Refresh token expired at {ExpiresAt}", refreshToken.ExpiresAt);

            throw new UnauthorizedAccessException("Refresh token expired");
        }

        var existingUser = await userRepository.GetById(refreshToken.UserId);

        if (existingUser is null)
        {
            logger.LogWarning("User with id {UserId} was not found", refreshToken.UserId);

            throw new KeyNotFoundException($"User with id {refreshToken.UserId} was not found");
        }

        var accessToken = tokenService.CreateAccesToken(existingUser);

        return new RefreshResult
        {
            AccessToken = accessToken
        };
    }
}