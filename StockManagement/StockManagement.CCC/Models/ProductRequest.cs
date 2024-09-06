namespace StockManagement.API.Models {
    public class ProductRequest {
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
