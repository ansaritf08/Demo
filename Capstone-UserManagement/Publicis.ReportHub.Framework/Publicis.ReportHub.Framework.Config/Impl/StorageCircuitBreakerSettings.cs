using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    class StorageCircuitBreakerSettings : IStorageCircuitBreakerSettings
    {
        private readonly IConfigProvider _configProvider;

        public StorageCircuitBreakerSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            ExceptionsAllowedBeforeBreakingBlobStorage = int.Parse(await _configProvider.GetConfigValueAsync(ConfigurationKeys.ExceptionsAllowedBeforeBreakingBlobStorage));

            DurationOfBreakInMinForBlobStorage = int.Parse(await _configProvider.GetConfigValueAsync(ConfigurationKeys.DurationOfBreakInMinForBlobStorage));
        }

        public int ExceptionsAllowedBeforeBreakingBlobStorage { get; set; }

        public int DurationOfBreakInMinForBlobStorage { get; set; }
    }
}
