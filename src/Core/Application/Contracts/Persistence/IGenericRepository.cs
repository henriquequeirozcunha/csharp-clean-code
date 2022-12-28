namespace Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> Add(T entity);
        Task Upadte(T entity);
        Task Remove(T entity);
        Task<bool> Exists(int id);
    }
}