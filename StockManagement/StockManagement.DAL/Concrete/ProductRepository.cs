using Microsoft.EntityFrameworkCore;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;

namespace StockManagement.DAL.Concrete {
    internal class ProductRepository : Repository<Product>, IProductRepository {
        public ProductRepository(DbContext context) : base(context) {
        }
    }
}
