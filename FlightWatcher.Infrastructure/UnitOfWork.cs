namespace FlightWatcher.Infrastructure
{
    public interface IUnitOfWork
    {
        IFlightRepository FlightRepository { get; }
        IBookmarkRepository BookmarkRepository { get; }
        IReminderRepository ReminderRepository { get; }
        INotificationRepository NotificationRepository { get; }

        T ExecuteTransaction<T>(Func<T> action);
        void Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public UnitOfWork(AppDbContext appDbContext, IHttpClientFactory httpclient, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _httpClientFactory = httpclient;
            _configuration = configuration;
        }

        private IFlightRepository _flightRepository;
        public IFlightRepository FlightRepository
        {
            get
            {
                _flightRepository ??= new FlightRepository(/*_appDbContext,*/ _httpClientFactory.CreateClient(), _configuration);
                return _flightRepository;
            }
        }

        private IBookmarkRepository _bookmarkRepository;
        public IBookmarkRepository BookmarkRepository
        {
            get
            {
                _bookmarkRepository ??= new BookmarkRepository(_appDbContext);
                return _bookmarkRepository;
            }
        }

        private INotificationRepository _notificationRepository;
        public INotificationRepository NotificationRepository
        {
            get
            {
                _notificationRepository ??= new NotificationRepository(_appDbContext);
                return _notificationRepository;
            }
        }

        private IReminderRepository _reminderRepository;
        public IReminderRepository ReminderRepository
        {
            get
            {
                _reminderRepository ??= new ReminderRepository(_appDbContext);
                return _reminderRepository;
            }
        }
        public T ExecuteTransaction<T>(Func<T> action)
        {
            var transaction = _appDbContext.Database.BeginTransaction();
            try
            {
                var result = action();
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Commit()
        {
            _appDbContext.SaveChanges();
        }

    }
}
