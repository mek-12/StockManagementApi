using Microsoft.Extensions.DependencyInjection;

namespace StockManagement.CCC.Extensions {
    public static class ServiceCollectionExtension {
        public static void RegisterStockManagementServices(this IServiceCollection services) {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(ass =>(bool)(ass?.GetName()?.FullName?.Contains("StockManagement")));
            services.AddAutoMapper(assemblies);
        }
    }
}
