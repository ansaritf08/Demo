using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Publicis.ReportHub.Framework.Config.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Messaging.Impl
{
    public abstract class MessageHandler<TMessage> : BackgroundService
    {
        protected ILogger<MessageHandler<TMessage>> _logger;

        protected string QueueName { get; set; }

        protected readonly IServiceBusSettings _serviceBusConfigSettings;

        public MessageHandler(ILogger<MessageHandler<TMessage>> logger, IServiceBusSettings serviceBusConfigSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusConfigSettings = serviceBusConfigSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messageProcessor = CreateServiceBusProcessor(QueueName);
            messageProcessor.ProcessMessageAsync += HandleMessageAsync;
            messageProcessor.ProcessErrorAsync += HandleReceivedExceptionAsync;

            _logger.LogDebug($"Starting message pump on queue {QueueName} in namespace {messageProcessor.FullyQualifiedNamespace}");

            await messageProcessor.StartProcessingAsync(stoppingToken);

            _logger.LogDebug("Message pump started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            _logger.LogDebug("Closing message pump");

            await messageProcessor.CloseAsync(cancellationToken: stoppingToken);

            _logger.LogDebug("Message pump closed : {Time}", DateTimeOffset.UtcNow);
        }

        private ServiceBusProcessor CreateServiceBusProcessor(string queueName)
        {
            var serviceBusClient = AuthenticateToAzureServiceBus();
            return serviceBusClient.CreateProcessor(queueName);
        }

        private ServiceBusClient AuthenticateToAzureServiceBus()
        {
            return new ServiceBusClient(_serviceBusConfigSettings.ServiceBusConnectionString);
        }

        private async Task HandleMessageAsync(ProcessMessageEventArgs processMessageEventArgs)
        {
            try
            {
                var rawMessageBody = Encoding.UTF8.GetString(processMessageEventArgs.Message.Body.ToArray());

                _logger.LogDebug("Received message {MessageId} with body {MessageBody}", processMessageEventArgs.Message.MessageId, rawMessageBody);

                var message = JsonConvert.DeserializeObject<TMessage>(rawMessageBody);

                if (message != null)
                {
                    await ProcessMessage(message, processMessageEventArgs.Message.MessageId,
                        processMessageEventArgs.Message.ApplicationProperties,
                        processMessageEventArgs.CancellationToken);
                }
                else
                {
                    _logger.LogError(
                        "Unable to deserialize to message contract {ContractName} for message {MessageBody}",
                        typeof(TMessage), rawMessageBody);
                }

                _logger.LogDebug("Message {MessageId} processed", processMessageEventArgs.Message.MessageId);

                await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to handle message");
                await processMessageEventArgs.AbandonMessageAsync(processMessageEventArgs.Message);
            }
        }

        private Task HandleReceivedExceptionAsync(ProcessErrorEventArgs exceptionEvent)
        {
            _logger.LogError(exceptionEvent.Exception, "Unable to process message");
            return Task.CompletedTask;
        }

        protected abstract Task ProcessMessage(TMessage message, string messageId, IReadOnlyDictionary<string, object> userProperties, CancellationToken cancellationToken);
    }
}
