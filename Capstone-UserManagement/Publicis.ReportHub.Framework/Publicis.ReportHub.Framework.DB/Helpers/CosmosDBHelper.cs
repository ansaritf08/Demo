using Publicis.ReportHub.Framework.Config.Impl;
using System.Collections.Generic;
using System.Linq;

namespace Publicis.ReportHub.Framework.DB.Helpers
{
    public static class CosmosDBHelper
    {
        public static string SelectDaprStoreName(string tenantName, IEnumerable<DaprCosmosStoreSetting> DaprCosmosStoreSettings)
        {
            string daprComponentSetting = null;
            daprComponentSetting = DaprCosmosStoreSettings.Where(settings => settings.TenantName.Equals(tenantName, System.StringComparison.InvariantCultureIgnoreCase))
                                                        ?.Select(settings => settings.DaprComponent)
                                                        .FirstOrDefault();

            return daprComponentSetting;
        }
    }
}
