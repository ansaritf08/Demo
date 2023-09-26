using System;
using System.Collections.Generic;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    [Serializable]
    public class TradeRegulatorVerificationLookup
    {
        public string TenantName { get; set; }

        public IEnumerable<string> TradeRegulatorNames { get; set; }

        public string id { get; set; }

        public DateTime RecordCreationDateTime { get; set; }

        public DateTime RecordUpdatedDateTime { get; set; }
    }
}