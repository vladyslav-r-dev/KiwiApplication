using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Patterns;

public class AuthRepository(AppDbContext db) : ICheckUserData
{
    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await db.Users.FirstOrDefaultAsync(x => x.Email == email);;
    }

    public async Task<UserEntity> GetUserById(Guid Id)
    {
        return await db.Users.FirstOrDefaultAsync(x => x.Id == Id);;
    }

    public Task<UserEntity> AddUser(UserEntity user)
    {
        db.Users.Add(user);
        return Task.FromResult(user);
    }
}