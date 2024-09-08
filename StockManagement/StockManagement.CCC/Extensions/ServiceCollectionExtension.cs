using Microsoft.Extensions.DependencyInjection;

namespace StockManagement.CCC.Extensions {
    public static class ServiceCollectionExtension {
        public static void RegisterDefaultServices(this IServiceCollection services) {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
