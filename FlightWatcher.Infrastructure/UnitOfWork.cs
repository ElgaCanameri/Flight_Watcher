namespace FlightWatcher.Infrastructure
{
    public interface IUnitOfWork
    {
        IFlightRepository FlightRepository { get; }
        IBookmarkRepository BookmarkRepository { get; }
        IReminderRepository ReminderRepository { get; }
        INotificationRepository NotificationRepository { get; }
        Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action);
        Task CommitAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public UnitOfWork(AppDbContext appDbContext, IHttpClientFactory httpclient, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _appDbContext = appDbContext;
            _httpClientFactory = httpclient;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        private IFlightRepository _flightRepository;
        public IFlightRepository FlightRepository
        {
            get
            {
                _flightRepository ??= new FlightRepository(_httpClientFactory.CreateClient(), _configuration, _memoryCache);
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
        public async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action)
        {
            await using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await action();

                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
