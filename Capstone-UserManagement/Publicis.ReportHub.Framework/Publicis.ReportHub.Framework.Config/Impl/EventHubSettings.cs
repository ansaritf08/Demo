using Microsoft.Extensions.Configuration;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    public class EventHubSettings :IEventHubSettings
    {
        private readonly IConfigProvider _configProvider;

        public EventHubSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }
        public string ehubNamespaceConnectionString { get; set; }
        public string eventHubName { get; set; }
        public string blobStorageConnectionString { get; set; }
        public string blobContainerName { get; set; }
       // public string[] ehubNamespaceConnectionStringList { get; set; }

        private async Task SetValues()
        {
            //connection= await _configProvider.GetConfigValueAsync(ConfigurationKeys.EhubNamespaceConnectionString).Get;
            // IConfigurationRoot _config = new ConfigurationBuilder()
            ehubNamespaceConnectionString = await _configProvider.GetConfigValueAsync(ConfigurationKeys.EhubNamespaceConnectionString);
            //ehubNamespaceConnectionStringList = await _configProvider.GetConfigSectionValueAsync("ehubNamespaceConnectionStringList");
            eventHubName = await _configProvider.GetConfigValueAsync(ConfigurationKeys.EventHubName);
            blobStorageConnectionString = await _configProvider.GetConfigValueAsync(ConfigurationKeys.BlobStorageConnectionString);
            blobContainerName = await _configProvider.GetConfigValueAsync(ConfigurationKeys.BlobContainerName);
        }

    }
}
