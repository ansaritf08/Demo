using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public static class CosmosDB
    {
        public static ComponentName Component(StageName stageName) => stageName switch
        {
            StageName.DataIngested => ComponentName.InBoundSubmission,
            StageName.DataTransformed => ComponentName.messageheader,
            StageName.DealsReceived => ComponentName.messageheader,
            StageName.DealsProcessed => ComponentName.messageheader,
            StageName.MessageGenerated => ComponentName.messagesubscriptionheader,
            StageName.MessageDelivered => ComponentName.messagesubscriptionheader,
            StageName.MessageAckReceived => ComponentName.messagesubscriptionheader,          
            _ => throw new ArgumentOutOfRangeException(nameof(stageName), $"Not expected direction value: {stageName}"),
        };
    }
}
