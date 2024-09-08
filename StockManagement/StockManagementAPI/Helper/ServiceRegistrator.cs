using System.Reflection;

namespace StockManagement.API.Helper {
    public static class ServiceRegistrator {
        public static void AddReferencedProjectServices(this IServiceCollection services, Assembly[] assemblies, IConfiguration configurationManager) {
            //  We are scanning all the referenced assemblies.
            foreach (var assembly in assemblies) {
                // We are retrieving all the types in each assembly.
                var types = assembly.GetTypes();

                // We are finding the static methods that take an 'IServiceCollection' parameter.
                var serviceRegistrationMethods = types
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(m => m.GetParameters().Any(p => p.ParameterType == typeof(IServiceCollection)));

                // Invoke methods
                foreach (var method in serviceRegistrationMethods) {
                    if(method.Name != "RegisterStockManagementServices") {
                        continue;
                    }
                    // Ensure that the first parameter of the method is 'IServiceCollection'.
                    var parameters = method.GetParameters();
                    if(parameters.Length == 0) {
                        return;
                    }
                    if(parameters.Length == 1) {
                        if (parameters[0].ParameterType == typeof(IServiceCollection)) {
                            method.Invoke(null, new object[] { services });
                        }
                    }
                    if (parameters.Length == 2) {
                        if (parameters[0].ParameterType == typeof(IServiceCollection) &&
                            parameters[1].ParameterType == typeof(IConfiguration)) {
                            method.Invoke(null, new object[] { services, configurationManager });
                        }
                    }
                }
            }
        }
    }
}
