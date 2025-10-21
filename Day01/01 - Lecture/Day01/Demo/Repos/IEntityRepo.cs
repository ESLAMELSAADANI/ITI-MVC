namespace Demo.Repos
{
    public interface IEntityRepo<T>
    {
        List<T> GetAll();
        T Get(int id);
        T Insert(T entity);
        T Update(T entity);
        T Details(int id);
        void Delete(int id);
        int Save();
    }
}
