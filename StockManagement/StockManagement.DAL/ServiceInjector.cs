using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.DAL.Concrete;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;
using Microsoft.Extensions.Configuration;
using StockManagement.CCC;

namespace StockManagement.DAL {
    public static class ServiceInjector {
        public static void RegisterDefaultServices(this IServiceCollection serviceCollection, IConfiguration configuration) {
            serviceCollection.AddDbContext<StockManagementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(Constants.DEFAULT_CONNECTION))); 
            serviceCollection.AddScoped<IRepository<Product>, Repository<Product>>();
            serviceCollection.AddScoped<IRepository<PriceHistory>, Repository<PriceHistory>>();
        }
    }
}
