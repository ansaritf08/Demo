using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.ServiceBus;
using PublisherPOC;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace HttpTrigger
{
    public  static class Function1
    {

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
           .SetBasePath(context.FunctionAppDirectory)
           // This gives you access to your application settings 
           // in your local development environment
           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           // This is what actually gets you the 
           // application settings in Azure
           .AddEnvironmentVariables()
           .Build();

            IQueueClient client = new QueueClient(config["ConnectionString"], config["Queue"]);
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
            Console.WriteLine("End Publishing the data to service bus");
            //Console.Read();

            return new OkObjectResult("End Publishing the data to service bus");
        }
    }
}
