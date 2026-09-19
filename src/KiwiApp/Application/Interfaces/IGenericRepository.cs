namespace KiwiApp.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetById(int id);
    
    Task<List<T>> GetAll();
    
    Task<T> Add(T entity);
    
    Task<T> Remove(T entity);
}