using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.DAL.Concrete;
using StockManagement.DAL.Core.Entities;
using StockManagement.DAL.Interfces;

namespace StockManagement.DAL {
    public static class ServiceInjector {
        public static void RegisterDefaultServices(this IServiceCollection serviceCollection) {
            serviceCollection.AddScoped<IRepository<Product>, Repository<Product>>();
            serviceCollection.AddScoped<IRepository<PriceHistory>, Repository<PriceHistory>>();
        }
    }
}
