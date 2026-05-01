namespace KiwiApp.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Passport { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}