namespace StockManagement.CCC.Entities
{
    public class Product {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<PriceHistory>? PriceHistories { get; set; }
    }
}
