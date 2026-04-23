using KiwiApp.Data;
using KiwiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Patterns;

public class AuthRepository : ICheckUserData
{
    private readonly AppDbContext _db;

    public AuthRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);;
    }

    public Task<UserEntity> AddUser(UserEntity user)
    {
        _db.Users.Add(user);
        return Task.FromResult(user);
    }
}