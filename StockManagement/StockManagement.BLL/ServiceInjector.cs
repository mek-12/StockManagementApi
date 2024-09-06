﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StockManagement.API.Application.Factories;
using StockManagement.BLL.Interfaces;
using StockManagement.BLL.Services;
using StockManagement.CCC.Entities;

namespace StockManagement.BLL {
    public static class ServiceInjector {
        public static void RegisterDefaultServices(this IServiceCollection serviceCollection) {
            serviceCollection.AddMemoryCache();
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
            serviceCollection.AddScoped<ICacheService>(provider =>
                provider.GetRequiredService<CacheServiceFactory>().CreateCacheService());
        }
    }
}
