using System.Collections.Generic;

namespace Publicis.ReportHub.Framework.Config.Impl
{
    public class DaprSettings
    {
        public string ShardContextComponentName { get; set; }

        public string ServiceBusPubSubComponentName { get; set; }
        
        public string ConsumeFromEh { get; set; }
        public string ConsumeForTrackAndTrace { get; set; }

        public string KeyVaultComponentName { get; set; }

        public string TradeRegulatorLookupDataKey { get; set; }

        public IEnumerable<DaprCosmosStoreSetting> DaprCosmosStoreSettings { get; set; }

        public IEnumerable<DaprBlobStoreSetting> DaprBlobStoreSetting { get; set; }
    }
}
