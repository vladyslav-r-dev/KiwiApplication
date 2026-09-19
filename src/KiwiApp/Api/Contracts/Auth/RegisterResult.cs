namespace KiwiApp.Api.Contracts.Auth;

public class RegisterResult
{
    public int UserId { get; set; }

    public string Email { get; set; } = string.Empty;
}