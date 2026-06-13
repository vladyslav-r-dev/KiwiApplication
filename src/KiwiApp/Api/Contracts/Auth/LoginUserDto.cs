namespace KiwiApp.Api.Contracts.Auth;

public class LoginUserDto
{
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}

public record RefreshRequest(
    string RefreshToken
);

public class GoogleUserDto
{
    public string IdToken { get; set; } = string.Empty;
}