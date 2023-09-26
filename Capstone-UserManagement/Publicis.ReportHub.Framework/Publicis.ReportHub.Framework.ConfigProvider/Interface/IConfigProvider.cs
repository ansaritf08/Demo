using System.Collections.Generic;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.ConfigProvider.Interface
{
    public interface IConfigProvider
    {
        Task<string> GetConfigValueAsync(string key);

        Task<string[]> GetConfigSectionValueAsync(string key);
    }
}

