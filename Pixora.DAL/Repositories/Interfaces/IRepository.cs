namespace Pixora.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        T Delete(int id);
        void Save();
    }
}
