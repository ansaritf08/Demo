using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CosmosTriggerr
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "roledetail",
            collectionName: "role",
            ConnectionStringSetting = "ConnectionString",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("Start publishing data to roleeh");
            var config = new ConfigurationBuilder()
          .SetBasePath(context.FunctionAppDirectory)
          // This gives you access to your application settings 
          // in your local development environment
          .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
          // This is what actually gets you the 
          // application settings in Azure
          .AddEnvironmentVariables()
          .Build();

            EventHubProducerClient eventHubClient = new EventHubProducerClient(config["EventHubConnectionString"], config["EventHub"]);
            List<EventData> lidata = new List<EventData>();
            string data = "";

            foreach (var document in input)
            {
                data = document.ToString();
                //DefaultSendOptions.PartitionKey = "Engineering";
                EventData eventData = new EventData(Encoding.UTF8.GetBytes(data));
                lidata.Add(eventData);
                await eventHubClient.SendAsync(lidata, cancellationToken: default);
            }

            await eventHubClient.CloseAsync();

            log.LogInformation("End publishing data to roleeh");
        }
    }
}
