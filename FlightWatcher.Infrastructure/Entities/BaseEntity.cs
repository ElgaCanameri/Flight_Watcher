namespace FlightWatcher.Infrastructure.Entities
{
    public abstract class DbEntity { }

    public abstract class BaseEntity<T1> : DbEntity
    {
        [Key]
        public T1 Id { get; set; }
    }
}
