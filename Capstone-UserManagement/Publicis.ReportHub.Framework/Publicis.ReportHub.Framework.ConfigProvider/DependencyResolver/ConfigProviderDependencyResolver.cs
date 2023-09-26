using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Publicis.ReportHub.Framework.Config.DependencyResolver;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Impl;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;

namespace Publicis.ReportHub.Framework.ConfigProvider.DependencyResolver
{
    public static class ConfigProviderDependencyResolver
    {
        public static void ResolveConfigProvider(this IServiceCollection services, IConfiguration configuration)
        {
            string environment = configuration["Environment"];

            if (environment.Equals("Local", System.StringComparison.InvariantCultureIgnoreCase)
                || environment.Equals("Dev", System.StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddSingleton<IConfigProvider, DefaultConfigProvider>();
            }
            else
            {
                services.Configure<DaprSettings>(configuration.GetSection("DaprSettings"));
                services.Configure<IEventHubSettings>(configuration.GetSection("ehubNamespaceConnectionStringList"));

                services.AddSingleton<DaprClient>(_ =>
                {
                    return new DaprClientBuilder().Build();
                });

                services.AddSingleton<IConfigProvider, KeyVaultConfigProvider>();
            }

            services.ResolveConfigDependencies();
        }
    }
}

