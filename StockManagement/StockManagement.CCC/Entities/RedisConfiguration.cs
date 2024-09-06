namespace StockManagement.API.Application.Entities {
    public class RedisConfiguration {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Password { get; set; }
        public int Database { get; set; }
    }
}
