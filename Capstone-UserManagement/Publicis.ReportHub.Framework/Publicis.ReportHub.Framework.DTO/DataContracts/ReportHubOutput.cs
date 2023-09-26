using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    [Serializable]
    public class ReportHubOutput
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string PartitionKey { get; set; }

        [Required]
        public string MessageCorrelationId { get; set; }

        [Required]
        public string BatchId { get; set; }

        [Required]
        public string TenantName { get; set; }

        [Required]
        public string ValidationType { get; set; }

        [Required]
        public string RecordStatus { get; set; }

        public DateTime RecordCreationDateTime { get; set; }

        public DateTime RecordUpdatedDateTime { get; set; }

        [Required]
        public IEnumerable<string> TradeRegulatorNames { get; set; }

        [Required]
        public GeneratorValuationInput GeneratorValuationInput { get; set; }

        public TransformedValuation TransformedValuation { get; set; }

        [Required]
        public string AssetClass { get; set; }

        public TNTMessage TNTMessage { get; set; }

        public Root Root { get; set; }
    }
}