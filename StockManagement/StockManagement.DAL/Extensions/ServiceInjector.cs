using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.DAL.Concrete;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;
using Microsoft.Extensions.Configuration;
using StockManagement.CCC;

namespace StockManagement.DAL.Extensions {
    public static class DALServiceInjector {
        public static void RegisterStockManagementServices(this IServiceCollection serviceCollection, IConfiguration configuration) {
            serviceCollection.AddDbContext<StockManagementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(Constants.DEFAULT_CONNECTION))); 
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();
        }
    }
}
