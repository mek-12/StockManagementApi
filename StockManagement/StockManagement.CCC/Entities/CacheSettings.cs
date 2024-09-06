namespace StockManagement.CCC.Entities
{
    public class CacheSettings
    {
        public string? CacheType { get; set; }
        public int DefaultCacheDuration { get; set; }
        public RedisConfiguration? RedisConfiguration { get; set; }
    }
}
