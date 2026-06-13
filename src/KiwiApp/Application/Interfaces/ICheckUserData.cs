using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Interfaces;

public interface ICheckUserData
{
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity?> GetUserByGoogleId(string payloadSubject);
}