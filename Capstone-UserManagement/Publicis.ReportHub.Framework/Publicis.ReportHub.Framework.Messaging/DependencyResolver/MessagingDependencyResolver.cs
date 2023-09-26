using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Messaging.Impl;
using Publicis.ReportHub.Framework.Messaging.Interface;

namespace Publicis.ReportHub.Framework.Messaging.DependencyResolver
{
    public static class MessagingDependencyResolver
    {

        public static void ResolveMessagingDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DaprSettings>(configuration.GetSection("DaprSettings"));
            services.Configure<DaprSettings>(configuration.GetSection("ehubNamespaceConnectionStringList"));
            services.AddSingleton<DaprClient>(_ =>
            {
                return new DaprClientBuilder().Build();
            });

            services.AddTransient<IMessageSender, MessageSender>();
        }
    }
}
