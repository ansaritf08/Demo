using System;
using System.ComponentModel.DataAnnotations;

namespace Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.DTO.DataContracts
{
    [Serializable]
    public class TRReportingOutput
    {
        [Required]
        public string MessageCorrelationId { get; set; }

        [Required]
        public string BatchId { get; set; }

        [Required]
        public string TenantName { get; set; }
    }
}
