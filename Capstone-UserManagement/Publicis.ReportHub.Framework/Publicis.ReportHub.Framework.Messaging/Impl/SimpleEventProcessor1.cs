using Microsoft.Azure.Cosmos;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.Messaging.Impl
{
    public class SimpleEventProcessor1 : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        private const string ContainerId = "kedascalibilty";
        private const string EndpointUrl = "https://apmstestpoc.documents.azure.com:443/";
        private const string AuthorizationKey = "6dsTgvEAMlrA27FYlYyEJYsEItWayoMbJLyXEMnkiL7WA2B6hNdioWBZrFtVVr3g6znnQjXNRvCqLIhJFwvV6A==";
        private const string ConnectionString = @"AccountEndpoint=https://apmstestpoc.documents.azure.com:443/;AccountKey=6dsTgvEAMlrA27FYlYyEJYsEItWayoMbJLyXEMnkiL7WA2B6hNdioWBZrFtVVr3g6znnQjXNRvCqLIhJFwvV6A==;";
        private const string DatabaseId = "ReportHubDB";
        CosmosClient cosmosClient = new CosmosClient(EndpointUrl, AuthorizationKey);

        public  Task  ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            try
            {
               
                Container container3 = cosmosClient.GetContainer(DatabaseId, ContainerId);
                XmlDocument xmldoc = new XmlDocument();
                foreach (var eventData in messages)
                {
                    var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    xmldoc.LoadXml(data);
                    var deal = xmldoc.SelectSingleNode("//Deal_Id").InnerText;
                    var ordernumber = xmldoc.SelectSingleNode("//OrderNumber").InnerText;
                    var logstr = $" enqueuetime : {DateTime.Now}  deal :{deal} , ordernumber : { ordernumber} , partiontionId :{context.PartitionId}";
                    var obj = new LogData { log=logstr, id= Guid.NewGuid().ToString()};
                    //Console.WriteLine($"Message received. Partition: '{context.PartitionId}', Data: '{data}'");
                    //await container3.CreateItemAsync(obj);

                  container3.CreateItemAsync<LogData>(obj).Wait();
                    


                    Console.WriteLine($" enqueuetime : {DateTime.Now} , deal :{deal} , ordernumber : { ordernumber} , partiontionId :{context.PartitionId}");
                }
               
            }
            catch(Exception ex)
            { }

            //return Task.CompletedTask;
            return  context.CheckpointAsync();
        }


        public class LogData
        {
            public string id { get; set; }
            public string log { get; set; }
        }


    }
}
