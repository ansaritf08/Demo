using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    public class CosmosConfigSettings : ICosmosConfigSettings
    {
        private readonly IConfigProvider _configProvider;

        public CosmosConfigSettings(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
            SetValues().Wait();
        }

        private async Task SetValues()
        {
            CosmosConnectionString = await _configProvider.GetConfigValueAsync(ConfigurationKeys.CosmosConnectionString);

            CosmosReferencedataContainername = await _configProvider.GetConfigValueAsync(ConfigurationKeys.CosmosReferencedataContainername);

            CosmosDataBase = await _configProvider.GetConfigValueAsync(ConfigurationKeys.CosmosDataBase);
        }

        public string CosmosReferencedataContainername { get; set; }
               
        public string CosmosConnectionString { get; set; }

        public string CosmosDataBase { get; set; }
    }
}
