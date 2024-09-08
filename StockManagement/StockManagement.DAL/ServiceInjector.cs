using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.DAL.Concrete;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;

namespace StockManagement.DAL {
    public static class ServiceInjector {
        public static void RegisterDefaultServices(this IServiceCollection serviceCollection) {
            serviceCollection.AddDbContext<StockManagementDbContext>(options =>
            options.UseSqlServer("connectionString")); 
            serviceCollection.AddScoped<IRepository<Product>, Repository<Product>>();
            serviceCollection.AddScoped<IRepository<PriceHistory>, Repository<PriceHistory>>();
        }
    }
}
