namespace KiwiApp.Application.Services;

public class UpdateUserResult
{
    public int UserId { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}