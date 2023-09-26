using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class FileReceived
    {
        public string StageName { get; set; }
        public string StageStatus { get; set; }
        public string JobID { get; set; }
        public string SourceSystemName { get; set; }
        public string TenantID { get; set; }
        public string InputLocation { get; set; }
        public string RowCount { get; set; }
        public string FileName { get; set; }
        public DateTime RecieveDateTime { get; set; }
        public string FailureReason { get; set; }
    }
}
