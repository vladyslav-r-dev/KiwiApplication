using KiwiApp.Application.Interfaces;
using KiwiApp.Application.UseCases;
using KiwiApp.Domain.Entities;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Infrastructure.Repositories;

public class AuthRepository(AppDbContext db) : GenericRepository<UserEntity>(db), ICheckUserData
{
    private readonly AppDbContext _db = db;

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);;
    }
}