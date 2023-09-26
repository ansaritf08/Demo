using Publicis.ReportHub.Framework.DTO.DataContracts;
using System;

namespace Publicis.ReportHub.Framework.DTO.MessageContracts
{
    [Serializable]
    public class MonikerRequestMessage : BaseMessage
    {
        public GeneratorValuationInput GeneratorValuationInput { get; set; }

        public string AssetClass { get; set; }

        public TNTMessage TNTMessage { get; set; }
    }
}