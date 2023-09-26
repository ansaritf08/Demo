using System;
using System.ComponentModel.DataAnnotations;

namespace Publicis.ReportHub.Framework.DTO.MessageContracts
{
    public class BaseMessage
    {
        [Required]
        public string CorrelationId { get; set; }

        [Required]
        public DateTime MessageInitiatedDateTime { get; set; }

        [Required]
        public string BatchId { get; set; }

        [Required]
        public string TenantName { get; set; }
    }
}