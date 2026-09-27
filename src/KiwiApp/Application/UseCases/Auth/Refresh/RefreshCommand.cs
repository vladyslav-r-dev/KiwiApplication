namespace KiwiApp.Application.UseCases.Auth.Refresh;

public class RefreshCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}