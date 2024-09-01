namespace StockManagementAPI.API.Models {
    public class PriceHistoryResponse {
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
