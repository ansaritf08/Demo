using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public  class ConstantsStageName
    {
            public const string DataIngested = "Data Ingested";
            public const string DataTransformed = "Data Transformed";
            public const string FileReceived = "File Received";
            public const string FileProcessing = "File Processing";
            public const string MessageReceived = "Message Received";
            public const string DealsReceived = "Deals Received";
            public const string DealsProcessed = "Deals Processed";
            public const string MessageGenerated = "Message Generated";
            public const string MessageDelivered = "Message Delivered";
            public const string MessageAckReceived = "Message Ack Received";
            public const string MessageReprocess = "Message Reprocess";
            public const string MessageAggregation = "Message Aggregation";
            public const string MessageAggregationStage1 = "Message Aggregation Stage1";
            public const string MessageAggregationStage2 = "Message Aggregation Stage2";
            public const string MessageAggregationStage2_AckReceived = "Message Aggregation Stage2 Ack Received";
            public const string MessageAggregationStage2_NackReceived = "Message Aggregation Stage2 Nack Received";
            public const string UnprocessedMessage = "Unprocessed Message";
            public const string MessageLogQuery = "MessageLogQuery";
            public const string ComprehensiveStatusQuery = "ComprehensiveStatusQuery";
            public const string CompoundMessageLogQuery = "CompoundMessageLogQuery";
            public const string ErrorLogQuery = "ErrorLogQuery";
            public const string CPBrokenFieldQuery = "CPBrokenFieldQuery";
            public const string CSVFileProcessStarted = "CSVFileProcessStarted";
            public const string CSVFileProcessCompleted = "CSVFileProcessCompleted";
            public const string HTTPMessageDelivered = "HTTP Message Delivered";
            public const string EOMMessageDelivered = "EOM Message Deilvered";
            public const string EODBatchDetails = "EOD Batch Details";
            public const string EOMBatchDetails = "EOM Batch Details";
            public const string AcerMessageLogQuery = "AcerMessageLogQuery";
            public const string TradeStatusQuery = "TradeStatusQuery";
            public const string FailureMessageQuery = "FailureMessageQuery";
            public const string MonikerStage = "Message Moniker";
            public const string TransformedStage = "Message Transformed";
        
    }
}
