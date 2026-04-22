using KiwiApp.Data;

namespace KiwiApp.Patterns;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}