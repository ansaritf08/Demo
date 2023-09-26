using System.Collections.Generic;

namespace Publicis.ReportHub.Framework.Observability.Helpers
{
    public static class LogMessages
    {
        public static KeyValuePair<int, string> ConfigProviderExceptionLog = new KeyValuePair<int, string>(1, "ConfigProviderException has occurred. Message:{0}");
        public static KeyValuePair<int, string> DatabaseExceptionLog = new KeyValuePair<int, string>(2, "DatabaseException has occurred. Message:{0}");
        public static KeyValuePair<int, string> MessageFrameworkExceptionLog = new KeyValuePair<int, string>(3, "MessageFrameworkException has occurred. Message:{0}");
        public static KeyValuePair<int, string> GeneralExceptionLog = new KeyValuePair<int, string>(4, "Exception has occurred. Message:{0}");
        public static KeyValuePair<int, string> StorageExceptionLog = new KeyValuePair<int, string>(5, "StorageException has occurred. Message:{0}");

        // Moniker logs
        public static KeyValuePair<int, string> MonikerRequestMessageProcessingStarted = new KeyValuePair<int, string>(100, "MonikerRequestMessage processing has started for messageId:{0}.");
        public static KeyValuePair<int, string> ValidMonikerRequestMessageProcessingStarted = new KeyValuePair<int, string>(101, "CorrelationId:{0}, MonikerRequestMessage processing has started.");
        public static KeyValuePair<int, string> MonikerRequestMessageProcessingCompled = new KeyValuePair<int, string>(102, "CorrelationId:{0}, MonikerRequestMessage processing has completed for messageId:{1}.");
        public static KeyValuePair<int, string> TradeRegulatorVerificationLookup = new KeyValuePair<int, string>(103, "CorrelationId:{0}, State store fetched TradeRegulatorVerificationLookup value:{1}");

        // Eligibility logs
        public static KeyValuePair<int, string> EligibiltyRequestMessageProcessingStarted = new KeyValuePair<int, string>(100, "EligibilityRequestMessage processing has started for messageId:{0}.");
        public static KeyValuePair<int, string> ValidEligibiltyRequestMessageProcessingStarted = new KeyValuePair<int, string>(101, "CorrelationId:{0}, EligibilityRequestMessage processing has started.");
        public static KeyValuePair<int, string> EligibilityRequestMessageProcessingCompled = new KeyValuePair<int, string>(102, "CorrelationId:{0}, EligibilityRequestMessage processing has completed for messageId:{1}.");
       // public static KeyValuePair<int, string> TradeRegulatorVerificationLookup = new KeyValuePair<int, string>(103, "CorrelationId:{0}, State store fetched TradeRegulatorVerificationLookup value:{1}");


        // Message framework logs
        public static KeyValuePair<int, string> messageSentLog = new KeyValuePair<int, string>(200, "Message has sent.");

        // Database framework logs
        public static KeyValuePair<int, string> cosmosInsertOperationStarted = new KeyValuePair<int, string>(300, "CorrelationId:{0}, Cosmos insert operation started for tenant:{1}");
        public static KeyValuePair<int, string> cosmosRecordInsertedLog = new KeyValuePair<int, string>(301, "CorrelationId:{0}, Cosmos record has created for tenantName:{1}, partitionKey:{2}, Id:{3}");
        public static KeyValuePair<int, string> cosmosRecordUpdatedLog = new KeyValuePair<int, string>(302, "Cosmos record has updated for tenantName:{0}, partitionKey:{1}, Id:{2}");
        public static KeyValuePair<int, string> FetchReportHubOutputOperationStarted = new KeyValuePair<int, string>(303, "CorrelationId:{0}, FetchReportHubOutput operation has started for container:{1} & CosmosRecordId:{2}");
        public static KeyValuePair<int, string> FetchReportHubOutputOperationCompleted = new KeyValuePair<int, string>(304, "CorrelationId:{0}, FetchReportHubOutput operation has completed for container:{1} & CosmosRecordId:{2}");
        public static KeyValuePair<int, string> ReportHubOutputRecordNotFound = new KeyValuePair<int, string>(305, "CorrelationId:{0}, Fetched ReportHubOutput cosmos record is null for container:{1} & CosmosRecordId:{2}");

        // Storage framework logs
        public static KeyValuePair<int, string> blobCreatedLog = new KeyValuePair<int, string>(400, "CorrelationId:{0}, TRReportingOutput blob has created for tenantName:{0}");

        // Transformer logs
        public static KeyValuePair<int, string> TransformerRequestMessageProcessingStarted = new KeyValuePair<int, string>(500, "TransformationServiceRequest processing has started for messageId:{0}.");
        public static KeyValuePair<int, string> ValidTransformerRequestMessageProcessingStarted = new KeyValuePair<int, string>(501, "CorrelationId:{0}, TransformationServiceRequest processing has started.");
        public static KeyValuePair<int, string> TransformerRequestMessageProcessingCompled = new KeyValuePair<int, string>(502, "CorrelationId:{0}, TransformationServiceRequest processing has completed for messageId:{1}.");
        public static KeyValuePair<int, string> TenantSpecificTransformationStarted = new KeyValuePair<int, string>(503, "CorrelationId:{0}, Transformation has started for tenant:{1}");
        public static KeyValuePair<int, string> TenantSpecificTransformationNotExecuted = new KeyValuePair<int, string>(504, "CorrelationId:{0}, Transformation has not executed for tenant:{1}");
        public static KeyValuePair<int, string> TenantSpecificTransformationCompleted = new KeyValuePair<int, string>(503, "CorrelationId:{0}, Transformation has completed for tenant:{1}");

        // Generator logs
        public static KeyValuePair<int, string> GeneratorRequestMessageProcessingStarted = new KeyValuePair<int, string>(600, "GeneratorRequestMessage processing has started for messageId:{0}.");
        public static KeyValuePair<int, string> GeneratorRequestMessageProcessingCompleted = new KeyValuePair<int, string>(601, "GeneratorRequestMessage processing has completed for messageId:{0}.");
    }
}
