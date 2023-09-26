using System;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    [Serializable]
    public class TransformedValuation
    {
        public string Value { get; set; }

        public string Currency { get; set; }
    }
}