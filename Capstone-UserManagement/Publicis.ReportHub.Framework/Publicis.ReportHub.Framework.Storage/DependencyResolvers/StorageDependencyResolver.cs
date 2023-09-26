using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Storage.Impl;
using Publicis.ReportHub.Framework.Storage.Interface;

namespace Publicis.ReportHub.Framework.Storage.DependencyResolvers
{
    public static class StorageDependencyResolver
    {
        public static void ResolveStorageDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DaprSettings>(configuration.GetSection("DaprSettings"));

            services.AddSingleton(_ =>
            {
                return new DaprClientBuilder().Build();
            });

            services.AddSingleton<IDaprStorageClient, DaprStorageClient>();
            services.AddSingleton<IBlobStorageClient, BlobStorageClient>();
        }
    }
}
