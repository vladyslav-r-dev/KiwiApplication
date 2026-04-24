using KiwiApp.Models;

namespace KiwiApp.Patterns;

public interface ICheckUserData
{
    Task<UserEntity?> GetUserByEmail(string Email);
    Task<UserEntity?> GetUserById(Guid Id);
    Task<UserEntity> AddUser(UserEntity user);
    
}