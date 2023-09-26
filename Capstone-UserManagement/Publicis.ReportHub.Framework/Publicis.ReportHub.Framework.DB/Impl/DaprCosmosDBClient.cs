using Dapr.Client;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Publicis.ReportHub.Framework.Config.Impl;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.DB.Exceptions;
using Publicis.ReportHub.Framework.DB.Helpers;
using Publicis.ReportHub.Framework.DB.Interface;
using Publicis.ReportHub.Framework.DTO.DataContracts;
using Publicis.ReportHub.Framework.Observability.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.DB.Impl
{
    public class DaprCosmosDBClient : IDaprCosmosDBClient
    {
        private readonly ILogger<DaprCosmosDBClient> _logger;
        private readonly DaprClient _daprClient;
        private readonly DaprSettings _daprSettings;
        private readonly IDBCircuitBreakerSettings _circuitBreakerSettings;


        public DaprCosmosDBClient(DaprClient daprClient, ILogger<DaprCosmosDBClient> logger, IOptions<DaprSettings> daprSettings, IDBCircuitBreakerSettings circuitBreakerSettings)
        {
            _daprClient = daprClient;
            _logger = logger;
            _daprSettings = daprSettings.Value;
            _circuitBreakerSettings = circuitBreakerSettings;
        }

        public async Task<TradeRegulatorVerificationLookup> FetchTradeRegulatorVerificationLookupRecord(string tenantName)
        {
            

            try
            {
                string storeName = _daprSettings.ShardContextComponentName;

                var metadata = new Dictionary<string, string>
                {
                    { "partitionKey", tenantName }
                };

                AsyncCircuitBreakerPolicy circuitBreakerPolicy = SetupCircuitBreakerPolicy();

                _logger.LogInformation($"CosmosDB circuit state: {circuitBreakerPolicy.CircuitState}");

                return await circuitBreakerPolicy.ExecuteAsync<TradeRegulatorVerificationLookup>(async () =>
                {
                    return await _daprClient.GetStateAsync<TradeRegulatorVerificationLookup>(storeName, _daprSettings.TradeRegulatorLookupDataKey, metadata: metadata);
                });

            }
            catch (Exception exception)
            {
                throw new DatabaseException("FetchTradeRegulatorVerificationLookupRecord", exception);
            }
        }

        public async Task InsertReportHubOutputRecords(IEnumerable<ReportHubOutput> reportHubOutputs, string tenantName, string correlationId)
        {
            try
            {
                _logger.LogInformation(LogMessages.cosmosInsertOperationStarted.Key, string.Format(LogMessages.cosmosInsertOperationStarted.Value, correlationId, tenantName));

                string storeName = CosmosDBHelper.SelectDaprStoreName(tenantName, _daprSettings.DaprCosmosStoreSettings);

                if (!string.IsNullOrEmpty(storeName))
                {
                    foreach (var reportHubOutput in reportHubOutputs)
                    {
                        var metadata = new Dictionary<string, string>
                        {
                            { "partitionKey", reportHubOutput.PartitionKey }
                        };

                        AsyncCircuitBreakerPolicy circuitBreakerPolicy = SetupCircuitBreakerPolicy();

                        _logger.LogInformation($"CosmosDB circuit state: {circuitBreakerPolicy.CircuitState}");

                        await circuitBreakerPolicy.ExecuteAsync(async () =>
                        {
                            await _daprClient.SaveStateAsync(storeName, reportHubOutput.id, reportHubOutput, metadata: metadata);
                        });

                        

                        string messageLog = string.Format(LogMessages.cosmosRecordInsertedLog.Value, correlationId, tenantName, reportHubOutput.PartitionKey, reportHubOutput.id);
                        _logger.LogInformation(LogMessages.cosmosRecordInsertedLog.Key, messageLog);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new DatabaseException("InsertReportHubOutputRecords", exception);
            }
        }

        public async Task UpsertReportHubOutputRecords(ReportHubOutput reportHubOutput, string tenantName)
        {
            try
            {
                string storeName = CosmosDBHelper.SelectDaprStoreName(tenantName, _daprSettings.DaprCosmosStoreSettings);

                if (!string.IsNullOrEmpty(storeName))
                {
                    var metadata = new Dictionary<string, string>
                        {
                            { "partitionKey", reportHubOutput.PartitionKey }
                        };

                    string key = reportHubOutput.id;
                    byte[] value = JsonSerializer.SerializeToUtf8Bytes(reportHubOutput);

                    StateTransactionRequest stateTransactionRequest = new StateTransactionRequest(key, value, StateOperationType.Upsert, metadata: metadata);

                    AsyncCircuitBreakerPolicy circuitBreakerPolicy = SetupCircuitBreakerPolicy();

                    _logger.LogInformation($"CosmosDB circuit state: {circuitBreakerPolicy.CircuitState}");

                    await circuitBreakerPolicy.ExecuteAsync(async () =>
                    {
                        await _daprClient.ExecuteStateTransactionAsync(storeName, new[] { stateTransactionRequest }, metadata: metadata);
                    });

                    string messageLog = string.Format(LogMessages.cosmosRecordUpdatedLog.Value, tenantName, reportHubOutput.PartitionKey, reportHubOutput.id);
                    _logger.LogInformation(LogMessages.cosmosRecordUpdatedLog.Key, messageLog);
                }
            }
            catch (Exception exception)
            {
                throw new DatabaseException("UpsertReportHubOutputRecords", exception);
            }
        }

        private AsyncCircuitBreakerPolicy SetupCircuitBreakerPolicy()
        {
            return Policy.Handle<TimeoutException>().Or<CosmosException>()
                                                              .CircuitBreakerAsync(_circuitBreakerSettings.ExceptionsAllowedBeforeBreakingCosmosDB,
                                                               TimeSpan.FromSeconds(_circuitBreakerSettings.DurationOfBreakInMinForCosmosDB),
                                                              (ex, t) =>
                                                              {
                                                                  _logger.LogWarning("CosmosDB circuit broken!");
                                                              },
                                                              () =>
                                                              {
                                                                  _logger.LogWarning("CosmosDB circuit reset!");
                                                              });
        }
    }
}
