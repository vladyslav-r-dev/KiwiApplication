namespace KiwiApp.Application.UseCases.Auth.Get;

public class MeResult
{
    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }
}