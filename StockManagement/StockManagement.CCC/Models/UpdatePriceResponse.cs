namespace StockManagement.CCC.Models {
    public class UpdatePriceResponse {
        public int ProductId { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ScheduledUpdate { get; set; }
    }
}
