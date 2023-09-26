using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EHTrigger
{
    public static class Function1
    {
        private static string ConnectionString;
        private static string Queue;
        private static string DatabaseId;
        private static string ContainerId;
        private static CosmosClient cosmosClient;

        [FunctionName("Function1")]
        public static async Task Run([EventHubTrigger("usereh", Connection = "ConnectionString")] EventData[] events, ExecutionContext context, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory)
                  .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();

                    cosmosClient = new CosmosClient(config["EndpointUrl"], config["AuthorizationKey"]);

                    var result = JsonConvert.DeserializeObject<PublisherPOC.User>(eventData.EventBody.ToString());
                    result.id = Guid.NewGuid().ToString();
                    Console.WriteLine($"Order Id is {result.UserId}, Order name is {result.UserName} and quantity is {result.Address}");
                    Container container = cosmosClient.GetContainer(config["DatabaseId"], config["ContainerId"]);
                    var item = container.CreateItemAsync<PublisherPOC.User>(result).GetAwaiter().GetResult();
                    log.LogInformation($"C# EventHub trigger function processed message: , for user :  {result.UserName}");



                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
