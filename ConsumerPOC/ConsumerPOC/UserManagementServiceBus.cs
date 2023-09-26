
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace ConsumerPOC
{
    public class UserManagementServiceBus : BackgroundService
    {
        private static string ConnectionString;
        private static string Queue;
        private static string DatabaseId ;
        private static string ContainerId;
        private static CosmosClient cosmosClient;
        private static IQueueClient client;
        IConfiguration _configuration;
        public UserManagementServiceBus(IConfiguration configuration)
        {
            _configuration= configuration;
            ConnectionString = _configuration["ConnectionString"];
            DatabaseId = _configuration["DatabaseId"].ToString();
            ContainerId = _configuration["containerId"];
            Queue = _configuration["Queue"];
            cosmosClient = new CosmosClient(_configuration["EndpointUrl"], _configuration["AuthorizationKey"]);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {           
            await Task.Factory.StartNew(() =>
            {
                client = new QueueClient(ConnectionString, Queue);
                var options = new MessageHandlerOptions(ExceptionMethod)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                client.RegisterMessageHandler(ExecuteMessageProcessing, options);
            });
            Console.Read();
        }
        private static async Task ExecuteMessageProcessing(Message message, CancellationToken arg2)
        
        {
            var result = JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(message.Body));
            result.id= Guid.NewGuid().ToString();
            Console.WriteLine($"Order Id is {result.UserId}, Order name is {result.UserName} and quantity is {result.Address}");
            Container container = cosmosClient.GetContainer(DatabaseId, ContainerId);
            var item = await container.CreateItemAsync<User>(result);
            Console.WriteLine($"Message with id {result.id} is logged to cosmos");
            await client.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static async Task ExceptionMethod(ExceptionReceivedEventArgs arg)
        {
            await Task.Run(() =>
           Console.WriteLine($"Error occured. Error is {arg.Exception.Message}")
           );
        }
    }
}
