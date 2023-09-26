using ConsumerPOC;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection.Metadata;

namespace ConsumerPOC
{

    public class UserManagementEventHub : ConsumerPOC.EventHandler
    {

        private static string ConnectionString;
        private static string Queue;
        private static string DatabaseId;
        private static string ContainerId;
        private static CosmosClient cosmosClient;
        private static IQueueClient client;
        IConfiguration _configuration;
        private readonly ILogger<EventHandler> _logger;
        public UserManagementEventHub(ILogger<EventHandler> logger, IConfiguration configuration):base(logger,  configuration)
        {
            _logger= logger;
            _configuration = configuration;
            ConnectionString = _configuration["ConnectionString"];
            DatabaseId = _configuration["DatabaseId"].ToString();
            ContainerId = _configuration["containerId"];
            Queue = _configuration["Queue"];
            cosmosClient = new CosmosClient(_configuration["EndpointUrl"], _configuration["AuthorizationKey"]);
        }

        protected override async Task ProcessMessage(string rawMessageBody, IDictionary<string, object> userProperties, CancellationToken cancellationToken)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var message = JsonConvert.DeserializeObject<User>(rawMessageBody, serializerSettings);
                message.id = Guid.NewGuid().ToString();
                Container container = cosmosClient.GetContainer(DatabaseId, ContainerId);
                var item = await container.CreateItemAsync<User>(message);
                Console.WriteLine($"Message with id {message.id} is logged to cosmos");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                throw;
            }
        }

    }
}
