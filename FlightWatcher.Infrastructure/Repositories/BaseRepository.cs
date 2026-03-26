namespace FlightWatcher.Infrastructure.Repositories
{
    public abstract class BaseRepository<T, T1> where T : BaseEntity<T1>
    {
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }
        public virtual async Task <T?> GetByIdAsync(T1 id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<bool> DeleteAsync(T1 id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return true;
        }
        public virtual async Task<T1> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id;
        }
        public virtual async Task <T?> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
        public virtual async Task <IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

    }
}
