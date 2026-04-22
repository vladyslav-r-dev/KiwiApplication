namespace KiwiApp.Patterns;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}