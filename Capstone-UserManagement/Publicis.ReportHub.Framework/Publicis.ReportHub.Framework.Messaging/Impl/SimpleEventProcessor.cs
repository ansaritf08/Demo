using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Primitives;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.Messaging.Impl
{
    public class SimpleEventProcessor
    {
        string connectionString = "Endpoint=sb://pocscalability.servicebus.windows.net/;SharedAccessKeyName=testscale;SharedAccessKey=fWLfuBqvQM7J42nj8dxGiwuDPV3dhybcbf3e8DykzcM=;EntityPath=testscale";
        string eventHubName = "testscale";
        private static readonly SendEventOptions DefaultSendOptions = new SendEventOptions();
        public  async Task  Publisher()
        {
            var producer = new EventHubProducerClient(connectionString, eventHubName);
            DefaultSendOptions.PartitionKey = "Key1";
            //DefaultSendOptions.PartitionId = "1";

            List<EventData> data = new List<EventData>();
            try
            {
                 EventDataBatch eventBatch =  producer.CreateBatchAsync().Result;
              
                //int.MaxValue
                for (var counter = 0; counter < 10; ++counter)
                {
                    var eventBody = new BinaryData($"Event Number: { counter + $" PartitonKey1 " }");
                    var eventData = new EventData(eventBody)  ;
                    data.Add(eventData);

                    //if (!eventBatch.TryAdd(eventData))
                    //{
                    //    // At this point, the batch is full but our last event was not
                    //    // accepted.  For our purposes, the event is unimportant so we
                    //    // will intentionally ignore it.  In a real-world scenario, a
                    //    // decision would have to be made as to whether the event should
                    //    // be dropped or published on its own.

                    //    break;
                    //}
                }

                // When the producer publishes the event, it will receive an
                // acknowledgment from the Event Hubs service; so long as there is no
                // exception thrown by this call, the service assumes responsibility for
                // delivery.  Your event data will be published to one of the Event Hub
                // partitions, though there may be a (very) slight delay until it is
                // available to be consumed.
                CancellationToken ct = default;

                await producer.SendAsync(data, DefaultSendOptions, ct);
            }
            catch (Exception ex)
            {
                // Transient failures will be automatically retried as part of the
                // operation. If this block is invoked, then the exception was either
                // fatal or all retries were exhausted without a successful publish.
            }
            finally
            {
                await producer.CloseAsync();
            }
        }
    }
}
