namespace KiwiApp.Application.UseCases.Auth.Login;

public class LoginResult
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}