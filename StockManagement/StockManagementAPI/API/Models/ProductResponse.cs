namespace StockManagementAPI.API.Models {
    public class ProductResponse {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<PriceHistoryResponse> PriceHistories { get; set; }
    }
}
