using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity?> GetUserByGoogleId(string payloadSubject);
    Task<UserEntity?> GetUserById(int userId);
}