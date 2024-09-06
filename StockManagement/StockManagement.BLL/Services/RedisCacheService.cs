using Newtonsoft.Json;
using StackExchange.Redis;
using StockManagement.BLL.Interfaces;

namespace StockManagement.BLL.Services {
    public class RedisCacheService : ICacheService {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis) {
            _database = redis.GetDatabase();
        }

        public T Get<T>(string key) {
            var value = _database.StringGet(key);
            return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default;
        }

        public void Set<T>(string key, T value, TimeSpan expiration) {
            var json = JsonConvert.SerializeObject(value);
            _database.StringSet(key, json, expiration);
        }

        public void Remove(string key) {
            _database.KeyDelete(key);
        }
    }
}
