using Microsoft.EntityFrameworkCore;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;

namespace StockManagement.DAL.Concrete {
    internal class PriceHistoryRepository : Repository<PriceHistory>, IPriceHistoryRepository{
        public PriceHistoryRepository(DbContext context) : base(context) {
        }
    }
}
