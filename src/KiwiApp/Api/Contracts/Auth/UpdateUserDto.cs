namespace KiwiApp.Api.Contracts.Auth;

public class UpdateUserDto
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
}