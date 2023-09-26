using Microsoft.Extensions.DependencyInjection;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Config.Interface;

namespace Publicis.ReportHub.Framework.Config.DependencyResolver
{
    public static class ConfigDependencyResolver
    {
        public static void ResolveConfigDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ICosmosConfigSettings, CosmosConfigSettings>();
            services.AddSingleton<IServiceBusSettings, ServiceBusSettings>();
            services.AddSingleton<IObservabilitySettings, ObservabilitySettings>();
            services.AddSingleton<IDBCircuitBreakerSettings, DBCircuitBreakerSettings>();
            services.AddSingleton<IServiceBusCircuitBreakerSettings, ServiceBusCircuitBreakerSettings>();
            services.AddSingleton<IStorageCircuitBreakerSettings, StorageCircuitBreakerSettings>();
            services.AddSingleton<IStorageSettings, StorageSettings>();
            services.AddSingleton<IEventHubSettings, EventHubSettings>();
        }
    }
}
