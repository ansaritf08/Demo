using Dapr.Client;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.Observability.Helpers;
using Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.DTO.DataContracts;
using Publicis.ReportHub.Framework.Storage.Exceptions;
using Publicis.ReportHub.Framework.Storage.Helpers;
using Publicis.ReportHub.Framework.Storage.Interface;
using System;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Storage.Impl
{
    public class DaprStorageClient : IDaprStorageClient
    {
        private readonly ILogger<DaprStorageClient> _logger;
        private readonly DaprClient _daprClient;
        private readonly DaprSettings _daprSettings;
        private readonly IStorageCircuitBreakerSettings _circuitBreakerSettings;

        public DaprStorageClient(DaprClient daprClient, ILogger<DaprStorageClient> logger, IOptions<DaprSettings> daprSettings, IStorageCircuitBreakerSettings circuitBreakerSettings)
        {
            _daprClient = daprClient;
            _logger = logger;
            _daprSettings = daprSettings.Value;
            _circuitBreakerSettings = circuitBreakerSettings;
        }

        public async Task InsertTRReportingOutput(TRReportingOutput TRReportingOutput, string tenantName)
        {
            try
            {
                string storeName = StorageHelper.SelectDaprStoreName(tenantName, _daprSettings.DaprBlobStoreSetting);

                if (!string.IsNullOrEmpty(storeName))
                {
                    AsyncCircuitBreakerPolicy circuitBreakerPolicy = SetupCircuitBreakerPolicy();

                    _logger.LogInformation($"Blob storage circuit state: {circuitBreakerPolicy.CircuitState}");

                    await circuitBreakerPolicy.ExecuteAsync(async () =>
                    {
                        await _daprClient.SaveStateAsync(storeName, TRReportingOutput.MessageCorrelationId, TRReportingOutput);
                        
                    });

                    string messageLog = string.Format(LogMessages.blobCreatedLog.Value, TRReportingOutput.MessageCorrelationId, tenantName);
                    _logger.LogInformation(LogMessages.blobCreatedLog.Key, messageLog);
                }
            }
            catch (Exception exception)
            {
                throw new StorageException("InsertTRReportingOutput", exception);
            }
        }

        private AsyncCircuitBreakerPolicy SetupCircuitBreakerPolicy()
        {
            return Policy.Handle<TimeoutException>()
                                                    .CircuitBreakerAsync(_circuitBreakerSettings.ExceptionsAllowedBeforeBreakingBlobStorage,
                                                    TimeSpan.FromSeconds(_circuitBreakerSettings.DurationOfBreakInMinForBlobStorage),
                                                    (ex, t) =>
                                                    {
                                                        _logger.LogWarning("Blob storage circuit broken!");
                                                    },
                                                    () =>
                                                    {
                                                        _logger.LogWarning("Blob storage circuit reset!");
                                                    });
        }
    }
}
