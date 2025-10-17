namespace Demo.Repos
{
    public interface IEntityRepo<T> : IDisposable
    {
        List<T> GetAll();
        T Get(int id);
        T Insert(T department);
        T Update(T department);
        T Details(int id);
        void Delete(int id);
        int Save();
    }
}
