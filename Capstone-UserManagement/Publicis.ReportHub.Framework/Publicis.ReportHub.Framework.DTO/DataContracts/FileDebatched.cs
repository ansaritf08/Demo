using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class FileDebatched
    {
        public string StageName { get; set; }
        public string TenantID { get; set; }
        public string JobID { get; set; }
        public string SourceSystemName { get; set; }
        public string DealID { get; set; }
        public string AssetClass { get; set; }
        public string TradeCorrelationID { get; set; }
        public string RawData { get; set; }
        public DateTime RecieveDateTime { get; set; }
        public string StageStatus { get; set; }
        public string FailureReason { get; set; }

    }
}
