using System;

namespace Publicis.ReportHub.Framework.DTO.MessageContracts
{
    [Serializable]
    public class TransformationServiceRequest : BaseMessage
    {
        public string CosmosRecordId { get; set; }
    }
}