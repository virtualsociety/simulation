using Microsoft.Extensions.DependencyInjection;
using Vs.Simulation.Core.Providers;

namespace Vs.Simulation.Core.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventLogging(this IServiceCollection services)
        {
            services.AddSingleton<IEventLoggingProvider, EventLoggingProvider>();
            return services;
        }
    }
}
