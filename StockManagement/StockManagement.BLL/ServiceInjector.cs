using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StockManagement.API.Application.Factories;
using StockManagement.BLL.Interfaces;
using StockManagement.BLL.Services;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Extensions;

namespace StockManagement.BLL {
    public static class ServiceInjector {
        public static void RegisterStockManagementServices(this IServiceCollection serviceCollection,IConfiguration configuration) {
            serviceCollection.AddMemoryCache();
            DALServiceInjector.RegisterStockManagementServices(serviceCollection, configuration);
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddSingleton<IConnectionMultiplexer>(provider => {
                var cacheSettings = provider.GetRequiredService<IOptions<CacheSettings>>().Value;
                var configurationOptions = new ConfigurationOptions {
                    EndPoints = { $"{cacheSettings.RedisConfiguration.Host}:{cacheSettings.RedisConfiguration.Port}" },
                    Password = cacheSettings.RedisConfiguration.Password,
                    DefaultDatabase = cacheSettings.RedisConfiguration.Database,
                    AbortOnConnectFail = false
                };
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
            serviceCollection.AddTransient<MemoryCacheService>();
            serviceCollection.AddTransient<RedisCacheService>();
            serviceCollection.AddScoped<CacheServiceFactory>();
            serviceCollection.AddScoped<ICacheService>(provider =>
                provider.GetService<CacheServiceFactory>().CreateCacheService());
        }
    }
}
