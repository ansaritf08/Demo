using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    [Serializable]
    public class MessageProcessed
    {
        public string TenantID { get; set; }
        public string JobID { get; set; }
        public string TradeCorrelationID { get; set; }
        public string OutboundMessageId { get; set; }
        public string MessageHeaderId { get; set; }
        public string SourceSystemName { get; set; }
        public string DealID { get; set; }
        public string AssetClass { get; set; }
        public string Regulation { get; set; }
        public string Regulator { get; set; }
        public string TradeRepositoryName { get; set; }
        public string StageName { get; set; }
        public string StageStatus { get; set; }
        public string MessageType { get; set; }
        public string RawData { get; set; }
        public DateTime RecieveDateTime { get; set; }
        public string FailureReason { get; set; }

    }
}
