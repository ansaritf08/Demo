using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using User = PublisherPOC.User;
using Microsoft.Extensions.Configuration;

namespace ServiceBusTrigger
{
    public class Function1
    {
        private static string ConnectionString;
        private static string Queue;
        private static string DatabaseId;
        private static string ContainerId;
        private static CosmosClient cosmosClient;
        private static IQueueClient client;

        [FunctionName("ServiceBusTrigger")]
        public void Run([ServiceBusTrigger("1wxtestqueue", Connection = "ConnectionString")]string myQueueItem, ExecutionContext context, ILogger log)
        {
            var config = new ConfigurationBuilder()
     .SetBasePath(context.FunctionAppDirectory)
     // This gives you access to your application settings 
     // in your local development environment
     .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
     // This is what actually gets you the 
     // application settings in Azure
     .AddEnvironmentVariables()
     .Build();

            cosmosClient = new CosmosClient(config["EndpointUrl"], config["AuthorizationKey"]);

            var result = JsonConvert.DeserializeObject<User>(myQueueItem);
            result.id = Guid.NewGuid().ToString();
            Console.WriteLine($"Order Id is {result.UserId}, Order name is {result.UserName} and quantity is {result.Address}");
            Container container = cosmosClient.GetContainer(config["DatabaseId"], config["ContainerId"]);
            var item = container.CreateItemAsync<User>(result).GetAwaiter().GetResult();
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem} , for user :  {result.UserName}");
            
        }
    }
}
