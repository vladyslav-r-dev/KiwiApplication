using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface ICheckUserData
{
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity> AddUser(UserEntity user);

    Task<UserEntity?> GetUserById(Guid userId);
}