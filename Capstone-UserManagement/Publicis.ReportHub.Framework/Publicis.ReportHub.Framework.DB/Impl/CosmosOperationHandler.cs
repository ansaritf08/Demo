using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.DB.Exceptions;
using Publicis.ReportHub.Framework.DB.Interface;
using Publicis.ReportHub.Framework.DTO.DataContracts;
using Publicis.ReportHub.Framework.Observability.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.DB.Impl
{
    public class CosmosOperationHandler : ICosmosOperationHandler
    {
        private readonly ICosmosConfigSettings _cosmosConfigSettings;
        private CosmosClient cosmosClient;
        private Database database;
        private readonly IDBCircuitBreakerSettings _circuitBreakerSettings;
        private readonly ILogger<CosmosOperationHandler> _logger;

        public CosmosOperationHandler(ICosmosConfigSettings cosmosConfigSettings, IDBCircuitBreakerSettings circuitBreakerSettings, ILogger<CosmosOperationHandler> logger)
        {
            _cosmosConfigSettings = cosmosConfigSettings;
            _circuitBreakerSettings = circuitBreakerSettings;
            _logger = logger;
            SetupCosmosClient();
        }

        public async Task<ReportHubOutput> FetchReportHubOutput(string containerName, string CosmosRecordId, string correlationId)
        {
            if (containerName is null)
            {
                throw new ArgumentNullException(nameof(containerName));
            }

            if (CosmosRecordId is null)
            {
                throw new ArgumentNullException(nameof(CosmosRecordId));
            }

            try
            {
                _logger.LogInformation(LogMessages.FetchReportHubOutputOperationStarted.Key,
                                      string.Format(LogMessages.FetchReportHubOutputOperationStarted.Value, correlationId, containerName, CosmosRecordId));

                Container container = database.GetContainer(containerName);

                string query = $"SELECT * FROM c where c.partitionKey = 'FX-ValidData' and c['value'].id = '{CosmosRecordId}'";
                QueryDefinition queryDefinition = new QueryDefinition(query);

                List<DaprBaseCosmosRecord<ReportHubOutput>> cosmosRecords = new List<DaprBaseCosmosRecord<ReportHubOutput>>();

                AsyncCircuitBreakerPolicy circuitBreakerPolicy = SetupCircuitBreakerPolicy();

                return await circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    FeedIterator<DaprBaseCosmosRecord<ReportHubOutput>> queryResultSetIterator = container.GetItemQueryIterator<DaprBaseCosmosRecord<ReportHubOutput>>(queryDefinition);

                    while (queryResultSetIterator.HasMoreResults)
                    {
                        FeedResponse<DaprBaseCosmosRecord<ReportHubOutput>> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                        cosmosRecords.AddRange(currentResultSet);
                    }

                    DaprBaseCosmosRecord<ReportHubOutput> reportHubOutput = cosmosRecords.FirstOrDefault();

                    _logger.LogInformation(LogMessages.FetchReportHubOutputOperationCompleted.Key,
                                  string.Format(LogMessages.FetchReportHubOutputOperationCompleted.Value, correlationId, containerName, CosmosRecordId));

                    return reportHubOutput?.value;
                });
            }
            catch (Exception exception)
            {
                throw new DatabaseException("FetchReportHubOutputs", exception);
            }
        }

        public void SetupCosmosClient()
        {
            cosmosClient = new CosmosClient(_cosmosConfigSettings.CosmosConnectionString);
            database = cosmosClient.GetDatabase(_cosmosConfigSettings.CosmosDataBase);
            SetupCircuitBreakerPolicy();
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
