namespace KiwiApp.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Passport { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string? Password { get; set; }

    public string? GoogleId { get; set; }

    public bool EmailConfirmed { get; set; }

    public string AuthProvider { get; set; } = "Local";

    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public string Role { get; set; } = "User";
}