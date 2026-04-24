namespace KiwiApp.DTOs;

public class LoginUserDto
{
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}

public record LoginRequest(
    string Email,
    string Password
);

public record RefreshRequest(
    string RefreshToken
);