using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    class DBCircuitBreakerSettings : IDBCircuitBreakerSettings
    {
        private readonly IConfigProvider _configProvider;

        public DBCircuitBreakerSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            ExceptionsAllowedBeforeBreakingCosmosDB = int.Parse(await _configProvider.GetConfigValueAsync(ConfigurationKeys.ExceptionsAllowedBeforeBreakingCosmosDB));

            DurationOfBreakInMinForCosmosDB = int.Parse(await _configProvider.GetConfigValueAsync(ConfigurationKeys.DurationOfBreakInMinForCosmosDB));
        }

        public int ExceptionsAllowedBeforeBreakingCosmosDB { get; set; }

        public int DurationOfBreakInMinForCosmosDB { get; set; }
    }
}
