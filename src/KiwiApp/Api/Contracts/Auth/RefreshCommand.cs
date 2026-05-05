namespace KiwiApp.Api.Contracts.Auth;

public class RefreshCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}