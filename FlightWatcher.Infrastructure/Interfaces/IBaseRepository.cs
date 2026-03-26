namespace FlightWatcher.Infrastructure.Interfaces
{
    public interface IBaseRepository<T,T1>
    {
        Task<T?> GetByIdAsync(T1 id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T1> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T1 id);
    }
}
