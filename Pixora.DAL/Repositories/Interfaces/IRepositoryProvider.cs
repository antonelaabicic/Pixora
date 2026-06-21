namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IRepositoryProvider
    {
        T GetRepository<T>() where T : class;
    }
}