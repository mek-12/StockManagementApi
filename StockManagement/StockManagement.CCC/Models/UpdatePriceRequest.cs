namespace StockManagement.CCC.Models {
    public class UpdatePriceRequest {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
    }
}
