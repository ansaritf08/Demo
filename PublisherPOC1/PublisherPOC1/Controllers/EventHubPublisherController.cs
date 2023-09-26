using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using PublisherPOC;
using System.Text;

namespace PublisherPOC1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventHubPublisherController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly ILogger<EventHubPublisherController> _logger;

        public EventHubPublisherController(ILogger<EventHubPublisherController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        [HttpPost(Name = "PushEventToEventHub")]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Start publishing the event to ");
            SendEventOptions DefaultSendOptions = new SendEventOptions();
            EventHubProducerClient eventHubClient = new EventHubProducerClient(_configuration["EventHubConnectionString"], _configuration["EventHub"]);
            List<EventData> lidata = new List<EventData>();
            string data = "";

            for (int i = 0; i < 2; i++)
            {
                var user = new User
                {
                    UserId = i.ToString(),
                    UserName = $"User_{i}",
                    Department = $"Department_{i}",
                    Address = $"Address_{i}",
                    MobileNumer = $"MobileNumber_{i}",
                    Age = $"{i}",
                };
                data = JsonConvert.SerializeObject(user);
                DefaultSendOptions.PartitionKey = "Engineering";
                EventData eventData = new EventData(Encoding.UTF8.GetBytes(data));
                lidata.Add(eventData);
            }

            await eventHubClient.SendAsync(lidata, DefaultSendOptions, cancellationToken: default);
            Console.WriteLine("End publishing the event to usereh");
            await eventHubClient.CloseAsync();
            return Ok("End publishing the event to usereh");
        }
    }
}
