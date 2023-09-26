using Publicis.ReportHub.Framework.Config.Impl;
using System.Collections.Generic;
using System.Linq;

namespace Publicis.ReportHub.Framework.Storage.Helpers
{
    public static class StorageHelper
    {
        public static string SelectDaprStoreName(string tenantName, IEnumerable<DaprBlobStoreSetting> DaprBlobStoreSetting)
        {
            string daprComponentSetting = null;
            daprComponentSetting = DaprBlobStoreSetting.Where(settings => settings.TenantName.Equals(tenantName, System.StringComparison.InvariantCultureIgnoreCase))
                                                        ?.Select(settings => settings.DaprComponent)
                                                        .FirstOrDefault();

            return daprComponentSetting;
        }
    }
}
