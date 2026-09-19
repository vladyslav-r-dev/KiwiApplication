namespace KiwiApp.Api.Contracts.Auth;

public class GoogleLoginCommand
{
    public string IdToken { get; set; } = string.Empty;
}