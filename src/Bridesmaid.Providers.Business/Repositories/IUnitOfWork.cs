namespace Bridesmaid.Providers.Business.Repositories;

public interface IUnitOfWork
{
    Task<bool> Save();
}