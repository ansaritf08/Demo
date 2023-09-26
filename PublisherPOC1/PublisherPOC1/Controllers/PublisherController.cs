using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using PublisherPOC;
using System.Text;

namespace PublisherPOC1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {

        IConfiguration _configuration;
        private readonly ILogger<PublisherController> _logger;

        public PublisherController(ILogger<PublisherController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "PushEventToServiceBus")]
        public async Task<IActionResult> Get()
        {
            IQueueClient client = new QueueClient(_configuration["ConnectionString"], _configuration["Queue"]);
            for (int i = 0; i < 1; i++)
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
                var messageBody = JsonConvert.SerializeObject(user);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                await client.SendAsync(message);
                Console.WriteLine($"Sending Message : {user.UserName.ToString()} ");
            }
            return Ok("End Publishing the data to service bus");
        }

     
    }
}