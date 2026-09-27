namespace KiwiApp.Application.UseCases.Auth.Get;

public class AdminUserResult
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Role { get; set; }
}