using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using PublisherPOC;

namespace HttpTriggerEH
{
    public static class Function1
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

            EventHubProducerClient eventHubClient = new EventHubProducerClient(config["ConnectionString"], config["EventHub"]);
            List<EventData> lidata = new List<EventData>();
            string data = "";

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
                data = JsonConvert.SerializeObject(user);
                //DefaultSendOptions.PartitionKey = "Engineering";
                EventData eventData = new EventData(Encoding.UTF8.GetBytes(data));
                lidata.Add(eventData);
            }

            await eventHubClient.SendAsync(lidata,  cancellationToken: default);
            Console.WriteLine("End publishing the event to usereh");
            await eventHubClient.CloseAsync();

            return new OkObjectResult("End publishing the event to usereh");
        }
    }
}
