namespace KiwiApp.Application.UseCases.Auth.Register;

public class RegisterResult
{
    public int UserId { get; set; }

    public string Email { get; set; } = string.Empty;
}