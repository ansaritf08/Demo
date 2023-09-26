using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    public class ServiceBusSettings :IServiceBusSettings
    {
        private readonly IConfigProvider _configProvider;

        public ServiceBusSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            ServiceBusConnectionString = await _configProvider.GetConfigValueAsync(ConfigurationKeys.ServiceBusConnectionString);

            ServiceBusMessageGeneratorQueueName = await _configProvider.GetConfigValueAsync(ConfigurationKeys.ServiceBusMessageGeneratorQueueName);

            ServiceBusMessageMonikerQueueName = await _configProvider.GetConfigValueAsync(ConfigurationKeys.ServiceBusMessageMonikerQueueName);

            ServiceBusEventGridQueueName = await _configProvider.GetConfigValueAsync(ConfigurationKeys.ServiceBusEventGridQueueName);
        }

        public string ServiceBusConnectionString { get; set; }

        public string ServiceBusMessageGeneratorQueueName { get; set; }

        public string ServiceBusMessageMonikerQueueName { get; set; }

        public string ServiceBusEventGridQueueName { get; set; }
    }
}
