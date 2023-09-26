namespace Publicis.ReportHub.Framework.Config
{
    public static class ConfigurationKeys
    {
        public const string EhubNamespaceConnectionString = "ehubNamespaceConnectionString";
        public const string EventHubName = "eventHubName";
        public const string BlobStorageConnectionString = "blobStorageConnectionString";
        public const string BlobContainerName = "blobContainerName";
        public const string CosmosConnectionString = "CosmosConnectionString";

        public const string CosmosReferencedataContainername = "CosmosReferencedataContainername";

        public const string CosmosDataBase = "CosmosDataBase";

        public const string AppInsightInstrumentationKey = "AppInsightInstrumentationKey";

        public const string LogLevel = "LogLevel";

        public const string ServiceBusConnectionString = "ServiceBusConnectionString";

        public const string ServiceBusMessageGeneratorQueueName = "ServiceBusMessageGeneratorQueueName";

        public const string ServiceBusMessageMonikerQueueName = "ServiceBusMessageMonikerQueueName";

        public const string ServiceBusEventGridQueueName = "ServiceBusEventGridQueueName";

        public const string ExceptionsAllowedBeforeBreakingServiceBus = "ExceptionsAllowedBeforeBreakingServiceBus";

        public const string DurationOfBreakInMinForServiceBus = "DurationOfBreakInMinForServiceBus";

        public const string ExceptionsAllowedBeforeBreakingCosmosDB = "ExceptionsAllowedBeforeBreakingCosmosDB";

        public const string DurationOfBreakInMinForCosmosDB = "DurationOfBreakInMinForCosmosDB";

        public const string ExceptionsAllowedBeforeBreakingBlobStorage = "ExceptionsAllowedBeforeBreakingCosmosDB";

        public const string DurationOfBreakInMinForBlobStorage = "DurationOfBreakInMinForBlobStorage";

        public const string StorageName = "StorageName";

        public const string StorageAccountKey = "StorageAccountKey";
    }
}
