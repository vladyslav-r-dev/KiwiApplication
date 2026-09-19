using KiwiApp.Application.Interfaces;
using KiwiApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KiwiApp.Application.UseCases;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>  where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public async Task<T?> GetById(int id)
    {
        return await _dbSet
            .FindAsync(id);
    }

    public Task<List<T>> GetAll()
    {
        return _dbSet.ToListAsync();
    }

    public Task<T> Add(T entity)
    {
        _dbSet.Add(entity);
        
        return Task.FromResult(entity);
    }

    public Task<T> Remove(T entity)
    {
        _dbSet.Remove(entity);
        
        return Task.FromResult(entity);
    }
}
