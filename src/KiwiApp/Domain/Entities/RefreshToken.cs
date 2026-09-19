namespace KiwiApp.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }
}