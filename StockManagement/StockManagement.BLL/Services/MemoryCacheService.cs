using Microsoft.Extensions.Caching.Memory;
using StockManagement.BLL.Interfaces;

namespace StockManagement.BLL.Services {
    public class MemoryCacheService : ICacheService {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key) {
            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan expiration) {
            _memoryCache.Set(key, value, expiration);
        }

        public void Remove(string key) {
            _memoryCache.Remove(key);
        }
    }
}
