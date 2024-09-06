using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace StockManagement.BLL.Helper {
    public static class ServiceRegistrator {
        public static void AddReferencedProjectServices(this IServiceCollection services, Assembly[] assemblies) {
            // Tüm referans verilen assembly'leri tarıyoruz
            foreach (var assembly in assemblies) {
                // Her bir assembly'deki tüm tipleri alıyoruz
                var types = assembly.GetTypes();

                // 'IServiceCollection' parametresi alan static metotları buluyoruz
                var serviceRegistrationMethods = types
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(m => m.GetParameters().Any(p => p.ParameterType == typeof(IServiceCollection)));

                // Bulunan metotları çağırıyoruz
                foreach (var method in serviceRegistrationMethods) {
                    // Metodun ilk parametresinin IServiceCollection olduğundan emin ol
                    if (method.GetParameters()[0].ParameterType == typeof(IServiceCollection)) {
                        // Metodu invoke ederek servisleri kaydediyoruz
                        method.Invoke(null, new object[] { services });
                    }
                }
            }
        }
    }
}
