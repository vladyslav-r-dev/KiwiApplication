using KiwiApp.Application.Interfaces;
using KiwiApp.Infrastructure.Persistence;

namespace KiwiApp.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}