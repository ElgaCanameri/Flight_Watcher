using FlightWatcher.Infrastructure;

namespace FlightWatcher.Application.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        public BaseService(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
        }
    }
}
