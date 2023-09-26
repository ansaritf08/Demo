using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    public class StorageSettings :IStorageSettings
    {
        private readonly IConfigProvider _configProvider;

        public StorageSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            StorageName = await _configProvider.GetConfigValueAsync(ConfigurationKeys.StorageName);

            StorageAccountKey = await _configProvider.GetConfigValueAsync(ConfigurationKeys.StorageAccountKey);
        }

        public string StorageName { get; set; }

        public string StorageAccountKey { get; set; }

    }
}
