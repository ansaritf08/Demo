using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public static class StageNameKey
    {
        public static StageName GetStageName(string stageName) => stageName switch
        {
            
            ConstantsStageName.DataIngested => StageName.DataIngested,
            ConstantsStageName.DataTransformed => StageName.DataTransformed,
            ConstantsStageName.DealsReceived => StageName.DealsReceived,
            ConstantsStageName.DealsProcessed => StageName.DealsProcessed,
            ConstantsStageName.MessageGenerated => StageName.MessageGenerated,
            ConstantsStageName.MessageDelivered => StageName.MessageDelivered,
            ConstantsStageName.MessageAckReceived => StageName.MessageAckReceived,
            _ => throw new ArgumentOutOfRangeException(nameof(stageName), $"Not expected direction value: {stageName}"),
        };

       
    }
}
