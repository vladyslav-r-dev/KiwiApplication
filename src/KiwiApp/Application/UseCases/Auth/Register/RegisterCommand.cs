namespace KiwiApp.Application.UseCases.Auth.Register;

public class RegisterCommand
{
    public string Name { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Passport { get; set; } = string.Empty;
}