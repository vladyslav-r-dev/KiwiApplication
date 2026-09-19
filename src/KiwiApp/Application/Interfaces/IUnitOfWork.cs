namespace KiwiApp.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}