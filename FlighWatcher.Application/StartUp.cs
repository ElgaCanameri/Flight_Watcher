using FlightWatcher.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightWatcher.Application
{
    public class StartUp
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            Infrastructure.StartUp.RegisterServices(services, configuration);
            services.AddScoped<IAuthService, AuthService>();    
            services.AddScoped<IFlightService, FlightService>();    
        }
    }
}
