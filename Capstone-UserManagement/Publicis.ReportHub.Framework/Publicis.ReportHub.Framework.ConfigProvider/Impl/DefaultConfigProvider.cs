using Microsoft.Extensions.Configuration;
using Publicis.ReportHub.Framework.ConfigProvider.Exceptions;
using Publicis.ReportHub.Framework.ConfigProvider.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.ConfigProvider.Impl
{
    public class DefaultConfigProvider : IConfigProvider
    {
        private readonly IConfiguration _configuration;

        public DefaultConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string[]> GetConfigSectionValueAsync(string key)
        {
            try
            {
                //var result = _configuration.GetSection("ehubNamespaceConnectionStringList"); // "Information"
                //var connectionList = result.Get<string[]>();
                //var x = _configuration.GetSection(key).GetChildren().Select(x => x.Value).ToArray();
                return await Task.FromResult(_configuration.GetSection(key).GetChildren().Select(x => x.Value).ToArray());
               
            }
            catch (Exception exception)
            {
                throw new ConfigProviderException(key, exception);
            }
        }

        public async Task<string> GetConfigValueAsync(string key)
        {
            try
            {
                return await Task.FromResult(_configuration.GetValue<string>(key));
               
            }
            catch (Exception exception)
            {
                throw new ConfigProviderException(key, exception);
            }
        }
    }
}
