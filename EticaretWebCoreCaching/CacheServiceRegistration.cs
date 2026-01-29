using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EticaretWebCoreCaching.Abstraction;
using EticaretWebCoreCaching.Services;

namespace EticaretWebCoreCaching
{
    public static class CacheServiceRegistration
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}
