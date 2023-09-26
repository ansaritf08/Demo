using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    class ServiceBusCircuitBreakerSettings : IServiceBusCircuitBreakerSettings
    {
        private readonly IConfigProvider _configProvider;

        public ServiceBusCircuitBreakerSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            ExceptionsAllowedBeforeBreakingServiceBus = int.Parse(await _configProvider.GetConfigValueAsync(ConfigurationKeys.ExceptionsAllowedBeforeBreakingServiceBus));

            DurationOfBreakInMinForServiceBus = int.Parse(await _configProvider.GetConfigValueAsync(ConfigurationKeys.DurationOfBreakInMinForServiceBus));
        }

        public int ExceptionsAllowedBeforeBreakingServiceBus { get; set; }

        public int DurationOfBreakInMinForServiceBus { get; set; }
    }
}
