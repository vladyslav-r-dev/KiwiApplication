namespace KiwiApp.Application.UseCases.Auth.Update;

public class UpdateUserCommand
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
}