using KiwiApp.Models;

namespace KiwiApp.Patterns;

public interface ICheckUserData
{
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity> AddUser(UserEntity user);

    Task<UserEntity?> GetUserById(Guid userId);
}