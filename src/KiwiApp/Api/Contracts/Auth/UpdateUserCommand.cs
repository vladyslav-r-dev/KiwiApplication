namespace KiwiApp.Api.Contracts.Auth;

public class UpdateUserCommand
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
}