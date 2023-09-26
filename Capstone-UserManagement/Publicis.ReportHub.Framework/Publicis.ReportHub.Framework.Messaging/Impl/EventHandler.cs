using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Publicis.ReportHub.Framework.Config.Interface;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Extensions.Configuration;

namespace Publicis.ReportHub.Framework.Messaging.Impl
{

    public abstract class EventHandler : BackgroundService
    {
        protected ILogger<EventHandler> _logger;

        protected string QueueName { get; set; }

        protected readonly IEventHubSettings _iEventHubSettings;
        static EventProcessorClient processor;

        public EventHandler(ILogger<EventHandler> logger, IEventHubSettings iEventHubSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _iEventHubSettings = iEventHubSettings;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            var connectionStringList = _iEventHubSettings.ehubNamespaceConnectionString;
            //for (int i = 0; i < connectionStringList.Length; i++)
            //{
                var ehconnect = connectionStringList;
                var messageProcessor = CreateEventHubProcessor(ehconnect);
                // Register handlers for processing events and handling errors
                messageProcessor.ProcessEventAsync += ProcessEventHandler;
                messageProcessor.ProcessErrorAsync += ProcessErrorHandler;

                _logger.LogDebug($"Starting message pump on queue {QueueName} in namespace {messageProcessor.FullyQualifiedNamespace}");

                await messageProcessor.StartProcessingAsync(stoppingToken);

                _logger.LogDebug("Message pump started");

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }

                _logger.LogDebug("Closing message pump");

                await messageProcessor.StopProcessingAsync(cancellationToken: stoppingToken);

                _logger.LogDebug("Message pump closed : {Time}", DateTimeOffset.UtcNow);
           // }
        }

        private EventProcessorClient CreateEventHubProcessor(string ehubNamespaceConnectionString)
        {
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            // Create a blob container client that the event processor will use 
            var storageClient = new BlobContainerClient(_iEventHubSettings.blobStorageConnectionString, _iEventHubSettings.blobContainerName);

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

                _logger.LogDebug("Received message {MessageId} with body {MessageBody}", eventArgs.Data.CorrelationId, rawMessageBody);
                //var serializerSettings = new JsonSerializerSettings
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //};
                //var x = JsonConvert.DeserializeObject<DaprRootMessage<TNTMessage>>(rawMessageBody, serializerSettings) ;
                //var message = JsonConvert.DeserializeObject<TMessage>(rawMessageBody, serializerSettings);

                if (rawMessageBody != null)
                {
                    await ProcessMessage(Encoding.UTF8.GetString(eventArgs.Data.EventBody.ToArray()),
                        eventArgs.Data.Properties,
                        eventArgs.CancellationToken);
           
                }
                else
                {
                    _logger.LogError(
                        "Unable to deserialize to message contract {ContractName} for message {MessageBody}",
                         rawMessageBody);
                }

                _logger.LogDebug("Message {MessageId} processed", eventArgs.Data.MessageId);

                await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to handle message");
                await Task.FromException(ex);
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            _logger.LogError(eventArgs.Exception, "Unable to process message");
            return Task.CompletedTask;
        }

        protected abstract Task ProcessMessage( string messageId, IDictionary<string, object> userProperties, CancellationToken cancellationToken);
    }
}
