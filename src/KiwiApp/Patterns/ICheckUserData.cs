using KiwiApp.Models;

namespace KiwiApp.Patterns;

public interface ICheckUserData
{
    Task<UserEntity?> GetUserByEmail(string Email);
    Task<UserEntity> AddUser(UserEntity user);
    
}