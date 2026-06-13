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
    ILogger<AuthService> logger, GoogleAuthService googleAuthService)
{
    public MeResult GetMe(ClaimsPrincipal user)
    {
        var userId = user.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var email = user.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        
        var role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

        return new MeResult
        {
            UserId = userId,
            Email = email,
            Role = role
        };
    }

    public async Task<LoginResult> LoginWithGoogle(GoogleLoginCommand command) // разобраться подробнее 
    {
        var payload = await googleAuthService.ValidateGoogleTokenAsync(command.IdToken);

        if (payload is null)
        {
            logger.LogWarning("Google login failed. Invalid Google token");

            throw new UnauthorizedAccessException("Invalid Google token");
        }

        if (!payload.EmailVerified)
        {
            logger.LogWarning("Google login failed. Email {Email} is not verified", payload.Email);

            throw new UnauthorizedAccessException("Google email is not verified");
        }

        var user = await checkUserData.GetUserByGoogleId(payload.Subject);

        if (user is null)
        {
            user = await checkUserData.GetUserByEmail(payload.Email);
        }

        if (user is null)
        {
            user = new UserEntity
            {
                FirstName = payload.GivenName ?? payload.Name ?? string.Empty,
                LastName = payload.FamilyName ?? string.Empty,
                Email = payload.Email,
                Password = null,
                Passport = string.Empty,
                AuthProvider = "Google",
                EmailConfirmed = true,
                GoogleId = payload.Subject,
                Role = "User"
            };

            await userRepository.Add(user);
            await unitOfWork.SaveChangesAsync();
        }
        else if (string.IsNullOrWhiteSpace(user.GoogleId))
        {
            user.GoogleId = payload.Subject;
            user.EmailConfirmed = true;

            if (user.AuthProvider != "Google")
            {
                user.AuthProvider = "Local";
            }

            await unitOfWork.SaveChangesAsync();
        }

        var accessToken = tokenService.CreateAccesToken(user);

        var refreshToken = Guid.NewGuid().ToString();

        await refreshTokenRepository.AddToken(refreshToken, user.Id);

        await unitOfWork.SaveChangesAsync();

        return new LoginResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
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

        if (existingUser?.Password is null)
        {
            throw new KeyNotFoundException("Invalid email or password");
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.Password);
        
        var newUser = new UserEntity
        {
            FirstName = command.Name,
            LastName = command.LastName,
            Email = command.Email,
            Password = hashedPassword,
            Passport = command.Passport,
            AuthProvider = "Local",
            EmailConfirmed = false,
            GoogleId = null
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

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Password, existingUser.Password);

        if (!isPasswordValid)
        {
            throw new KeyNotFoundException("Invalid email or password");
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