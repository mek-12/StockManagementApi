using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;

namespace StockManagement.DAL.Concrete {
    internal class PriceHistoryRepository : Repository<PriceHistory>, IPriceHistoryRepository{
        public PriceHistoryRepository(StockManagementDbContext context) : base(context) {
        }
    }
}
