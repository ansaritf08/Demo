using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Text;


namespace ConsumerPOC
{

    public abstract class EventHandler : BackgroundService
    {


        protected string eventHubName;
        string ehubNamespaceConnectionString;
        string blobStorageConnectionString;
        string blobContainerName;
        IConfiguration _configuration;
        private readonly ILogger<EventHandler> _logger;
        static EventProcessorClient processor;


        public EventHandler(ILogger<EventHandler> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
            eventHubName = _configuration["eventHubName"];
            ehubNamespaceConnectionString = _configuration["ehubNamespaceConnectionString"];
            blobStorageConnectionString = _configuration["blobStorageConnectionString"];
            blobContainerName = _configuration["blobContainerName"];
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           // var connectionString = ehubNamespaceConnectionString;

                var messageProcessor = CreateEventHubProcessor(ehubNamespaceConnectionString);
                // Register handlers for processing events and handling errors
                messageProcessor.ProcessEventAsync += ProcessEventHandler;
                messageProcessor.ProcessErrorAsync += ProcessErrorHandler;

                Console.WriteLine($"Starting message pump on queue {eventHubName} in namespace {messageProcessor.FullyQualifiedNamespace}");

                await messageProcessor.StartProcessingAsync(stoppingToken);

            //Console.WriteLine("Message pump started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30));
            }

            //Console.WriteLine("Closing message pump");

            await messageProcessor.StopProcessingAsync(cancellationToken: stoppingToken);

            Console.WriteLine("Message pump closed : {Time}", DateTimeOffset.UtcNow);
           
        }

        private EventProcessorClient CreateEventHubProcessor(string ehubNamespaceConnectionString)
        {
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // Create a blob container client that the event processor will use 
            var storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

            // Create an event processor client to process events in the event hub
            //  processor = new EventProcessorClient(storageClient, consumerGroup, _iEventHubSettings.ehubNamespaceConnectionString, _iEventHubSettings.eventHubName);
            processor = new EventProcessorClient(storageClient, consumerGroup, ehubNamespaceConnectionString);
            //var x = new EventProcessorHost
            // var x = new EventProcessorClient()
            return processor;       
            // ProcessEventHandler(processor.ProcessEventAsync);
        }

        private async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            try
            {             
                var rawMessageBody = Encoding.UTF8.GetString(eventArgs.Data.EventBody.ToArray());

                //Console.WriteLine("Received message {MessageId} with body {MessageBody}", eventArgs.Data.CorrelationId, rawMessageBody);

                if (rawMessageBody != null)
                {
                    await ProcessMessage(Encoding.UTF8.GetString(eventArgs.Data.EventBody.ToArray()),
                        eventArgs.Data.Properties,
                        eventArgs.CancellationToken);         
                }
                else
                {
                    Console.WriteLine(
                        "Unable to deserialize to message contract {ContractName} for message {MessageBody}",
                         rawMessageBody);
                }

                //Console.WriteLine("Message {MessageId} processed", eventArgs.Data.MessageId);

                await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine( "Unable to handle message");
                await Task.FromException(ex);
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine( "Unable to process message");
            return Task.CompletedTask;
        }

        protected abstract Task ProcessMessage( string messageId, IDictionary<string, object> userProperties, CancellationToken cancellationToken);
    }
}
