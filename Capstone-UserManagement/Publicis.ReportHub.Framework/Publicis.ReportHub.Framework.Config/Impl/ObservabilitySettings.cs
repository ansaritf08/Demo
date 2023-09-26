using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    public class ObservabilitySettings : IObservabilitySettings
    {
        private readonly IConfigProvider _configProvider;

        public ObservabilitySettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            AppInsightInstrumentationKey = await _configProvider.GetConfigValueAsync(ConfigurationKeys.AppInsightInstrumentationKey);
            LogLevel = await _configProvider.GetConfigValueAsync(ConfigurationKeys.LogLevel);
        }

        public string AppInsightInstrumentationKey { get; set; }

        public string LogLevel { get; set; }
    }
}
