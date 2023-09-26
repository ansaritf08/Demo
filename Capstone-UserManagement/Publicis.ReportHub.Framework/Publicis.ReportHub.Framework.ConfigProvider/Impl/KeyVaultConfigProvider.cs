using Dapr.Client;
using Microsoft.Extensions.Options;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.ConfigProvider.Exceptions;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.ConfigProvider.Impl
{
    public class KeyVaultConfigProvider : IConfigProvider
    {
        private readonly DaprClient _daprClient;
        private readonly DaprSettings _daprSettings;

        public KeyVaultConfigProvider(DaprClient daprClient, IOptions<DaprSettings> daprSettings)
        {
            _daprClient = daprClient;
            _daprSettings = daprSettings.Value;
        }

        public Task<string[]> GetConfigSectionValueAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetConfigValueAsync(string key)
        {
            try
            {
                var secretKeyValuePairs = await _daprClient.GetSecretAsync(_daprSettings.KeyVaultComponentName, key);
                var secretKeyValuePair = secretKeyValuePairs.First();
                return secretKeyValuePair.Value;
            }
            catch (Exception exception)
            {
                throw new ConfigProviderException(key, exception);
            }
        }
    }
}