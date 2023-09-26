using Azure.Messaging.ServiceBus;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.Messaging.Exceptions;
using Publicis.ReportHub.Framework.Messaging.Interface;
using Publicis.ReportHub.Framework.Observability.Helpers;
using System;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Messaging.Impl
{
    public class MessageSender : IMessageSender
    {
        private readonly ILogger<MessageSender> _logger;
        private readonly DaprClient _daprClient;
        private readonly DaprSettings _daprSettings;
        private readonly IServiceBusCircuitBreakerSettings _circuitBreakerSettings;

        public MessageSender(ILogger<MessageSender> logger, DaprClient daprClient, IOptions<DaprSettings> daprSettings, IServiceBusCircuitBreakerSettings circuitBreakerSettings)
        {
            _logger = logger;
            _daprClient = daprClient;
            _daprSettings = daprSettings.Value;
            _circuitBreakerSettings = circuitBreakerSettings;
        }

        public async Task SendMessage<T>(string queueName, T message)
        {
            try
            {
                AsyncCircuitBreakerPolicy circuitBreakerPolicy = Policy.Handle<TimeoutException>().Or<ServiceBusException>()
                                                                  .CircuitBreakerAsync(_circuitBreakerSettings.ExceptionsAllowedBeforeBreakingServiceBus, 
                                                                   TimeSpan.FromSeconds(_circuitBreakerSettings.DurationOfBreakInMinForServiceBus),
                                                                  (ex, t) =>
                                                                  {
                                                                      _logger.LogWarning("Service bus circuit broken!");
                                                                  },
                                                                  () =>
                                                                  {
                                                                      _logger.LogWarning("Service bus circuit reset!");
                                                                  });


                _logger.LogInformation($"Service bus circuit state: {circuitBreakerPolicy.CircuitState}");

                await circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    await _daprClient.PublishEventAsync<T>(_daprSettings.ConsumeFromEh, queueName, message);
                    await _daprClient.PublishEventAsync<T>(_daprSettings.ConsumeForTrackAndTrace, "trackandtrace", message);
                });
                
                _logger.LogInformation(LogMessages.messageSentLog.Key, LogMessages.messageSentLog.Value);
            }
            catch (Exception exception)
            {
                throw new MessageFrameworkException("Send", typeof(T).Name, message.ToString(), exception);
            }
        }
    }
}
