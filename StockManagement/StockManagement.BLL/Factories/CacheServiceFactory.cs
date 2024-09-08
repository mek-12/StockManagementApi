using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StockManagement.BLL.Interfaces;
using StockManagement.BLL.Services;
using StockManagement.CCC;
using StockManagement.CCC.Entities;

namespace StockManagement.API.Application.Factories {
    public class CacheServiceFactory {
        private readonly CacheSettings _cacheSettings;
        private readonly IServiceProvider _serviceProvider;

        public CacheServiceFactory(IOptions<CacheSettings> cacheSettings, IServiceProvider serviceProvider) {
            _cacheSettings = cacheSettings.Value;
            _serviceProvider = serviceProvider;
        }

        public ICacheService CreateCacheService() {
            return _cacheSettings.CacheType 
                switch {
                Constants.REDIS => _serviceProvider.GetRequiredService<RedisCacheService>(),
                _ => _serviceProvider.GetRequiredService<MemoryCacheService>(),
            };
        }
    }
}
