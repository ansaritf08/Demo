using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.DB.Impl;
using Publicis.ReportHub.Framework.DB.Interface;

namespace Publicis.ReportHub.Framework.DB.DependencyResolvers
{
    public static class DatabaseDependencyResolver
    {
        public static void ResolveDBDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosConfigSettings>(configuration.GetSection("CosmosConfigSettings"));
            services.Configure<DaprSettings>(configuration.GetSection("DaprSettings"));
            services.Configure<DaprSettings>(configuration.GetSection("ehubNamespaceConnectionString"));
            

            services.AddSingleton<DaprClient>(_ =>
            {
                return new DaprClientBuilder().Build();
            });

            services.AddSingleton<IDaprCosmosDBClient, DaprCosmosDBClient>();
            services.AddSingleton<ICosmosOperationHandler, CosmosOperationHandler>();
        }
    }
}
