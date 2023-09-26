using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.DependencyResolver;
//using Publicis.ReportHub.Framework.ConfigProvider.DependencyResolver;
using Publicis.ReportHub.Framework.ConfigProvider.Exceptions;
using Publicis.ReportHub.Framework.DB.DependencyResolvers;
using Publicis.ReportHub.Framework.Messaging.DependencyResolver;
using Publicis.ReportHub.Framework.Observability.DependencyResolvers;
//using Publicis.ReportHub.Framework.Messaging.DependencyResolver;
//using Publicis.ReportHub.Framework.Observability.DependencyResolvers;
using Publicis.ReportHub.Framework.Observability.Helpers;
using Publicis.ReportHub.Framework.Storage.DependencyResolvers;
using Publicis.ReportHub.Framework.Storage.Impl;
using Publicis.ReportHub.Framework.Storage.Interface;
//using Publicis.ReportHub.Framework.Storage.DependencyResolvers;
//using Publicis.ReportHub.Framework.Storage.DependencyResolvers;
using System;
using System.Threading.Tasks;


namespace Publicis.ReportHub.Services.MessageEligibility
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ServiceBusHostBuilder(args);
                IServiceCollection services = new ServiceCollection();

                var serviceProvider = services.BuildServiceProvider();

                var _telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();

                _telemetryClient.Flush();
                Task.Delay(5000).Wait();
            }
            catch (ConfigProviderException configProviderException)
            {
                var logMessage = string.Format(LogMessages.ConfigProviderExceptionLog.Value, configProviderException.Message);
                Console.WriteLine(logMessage);
                throw;
            }
            catch (Exception exception)
            {
                var logMessage = string.Format(LogMessages.GeneralExceptionLog.Value, exception.Message);
                Console.WriteLine(logMessage);
                throw;
            }
        }

        private static void ServiceBusHostBuilder(string[] args)
        {


            Host.CreateDefaultBuilder(args)
               .UseWindowsService() 
               .ConfigureHostConfiguration(configHost =>
               {
                   configHost.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddSingleton<IConfiguration>(provider => hostContext.Configuration);

                   services.AddOptions();
                   services.ResolveConfigProvider(hostContext.Configuration);

                   var serviceProvider = services.BuildServiceProvider();
                   var observabilitySettings = serviceProvider.GetRequiredService<IObservabilitySettings>();

                   services.ResolveObservabilityDependency(observabilitySettings);
                   services.ResolveDBDependency(hostContext.Configuration);

                   //services.ResolveStorageDependency(hostContext.Configuration);
                   services.AddSingleton<IDaprStorageClient, DaprStorageClient>();
                   services.AddSingleton<IBlobStorageClient, BlobStorageClient>();

                   services.ResolveMessagingDependency(hostContext.Configuration);
                   services.AddHostedService<UserManagementProcessor>();
               }).Build().Run();
        }
    }
}

