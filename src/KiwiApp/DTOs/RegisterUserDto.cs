namespace KiwiApp.DTOs;

public class RegisterUserDto
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null;
    
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    public string Passport { get; set; } = null!;
}