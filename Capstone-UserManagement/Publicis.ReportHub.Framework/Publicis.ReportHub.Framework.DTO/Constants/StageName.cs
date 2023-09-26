using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public enum StageName
    {
        DataIngested,
        DataTransformed,
        MessageGenerated,
        MessageDelivered,
        MessageAckReceived,
        DealsReceived,
        DealsProcessed
    }
}
