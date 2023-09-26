using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.Messaging.Impl
{
    public abstract class SimpleEventConsumer : BackgroundService
    {
        string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=apmstestapppoc;AccountKey=+A5EH8OUHkRY5aWrXagw1Y3sLQd/+BlhrxThiGVbKyHcXoEhK0jDzG6z1XIz/QbcJD0KvDZEgRPu/w20MS/uVg==;EndpointSuffix=core.windows.net";
        string blobContainerName = "idempotency";

        string eventHubsConnectionString = "Endpoint=sb://messageprocessing.servicebus.windows.net/;SharedAccessKeyName=listeninboundmsg;SharedAccessKey=kb1WOpgJVESNVprgamQLQc8RRnDgzHUN7TJ8gUblLdM=;EntityPath=inboundtransformationeh";
        string eventHubName = "inboundtransformationeh";
        string consumerGroup = "$Default";
         static Int64 cacheValueParition0 ;
        static Int64 cacheValuePartition1;


        public bool flag = false;

       public async Task processEventHandler(ProcessEventArgs args)
        {          
            try
            {

               
                var partitionEventCount = new ConcurrentDictionary<string, int>();
                // If the cancellation token is signaled, then the
                // processor has been asked to stop.  It will invoke
                // this handler with any events that were in flight;
                // these will not be lost if not processed.
                //
                // It is up to the handler to decide whether to take
                // action to process the event or to cancel immediately.

                if (args.CancellationToken.IsCancellationRequested)
                {
                    return;

                }
      
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine($"{args.Data.Offset} and {args.Data.SequenceNumber} and  {args.Partition.PartitionId}");

                var EnqueuedTime = args.Data.EnqueuedTime.ToString();
                string partition = args.Partition.PartitionId;

                cacheValueParition0 = 55834825120;
                if (partition == "0")
                {
                    flag = cacheValueParition0 < Convert.ToInt64(args.Data.Offset) ? false: true;
                    cacheValueParition0=Convert.ToInt64(args.Data.Offset);
                }
                else if (partition == "1")
                {
                    flag = cacheValuePartition1 < Convert.ToInt64(args.Data.Offset) ? false : true;
                    cacheValuePartition1 = Convert.ToInt64(args.Data.Offset);
                }

                byte[] eventBody = args.Data.EventBody.ToArray();
                Debug.WriteLine($"Event from partition { partition } with length { eventBody.Length }.");

                if (!flag)

                {
                    await ProcessMessage(Encoding.UTF8.GetString(args.Data.EventBody.ToArray()), partition, EnqueuedTime,
              args.Data.Properties,
              args.CancellationToken);

                }

                int eventsSinceLastCheckpoint = partitionEventCount.AddOrUpdate(
                    key: partition,
                    addValue: 1,
                    updateValueFactory: (_, currentCount) => currentCount + 1);

                await args.UpdateCheckpointAsync();


                //if (eventsSinceLastCheckpoint >= 50)
                //{
                //    await args.UpdateCheckpointAsync();
                //    partitionEventCount[partition] = 0;
                //}
            }
            catch
            {
                // It is very important that you always guard against
                // exceptions in your handler code; the processor does
                // not have enough understanding of your code to
                // determine the correct action to take.  Any
                // exceptions from your handlers go uncaught by
                // the processor and will NOT be redirected to
                // the error handler.
            }
        }

       public Task processErrorHandler(ProcessErrorEventArgs args)
        {
            try
            {
                Debug.WriteLine("Error in the EventProcessorClient");
                Debug.WriteLine($"\tOperation: { args.Operation }");
                Debug.WriteLine($"\tException: { args.Exception }");
                Debug.WriteLine("");
            }
            catch
            {
                // It is very important that you always guard against
                // exceptions in your handler code; the processor does
                // not have enough understanding of your code to
                // determine the correct action to take.  Any
                // exceptions from your handlers go uncaught by
                // the processor and will NOT be handled in any
                // way.
            }

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var storageClient = new BlobContainerClient(
                storageConnectionString,
                blobContainerName);

            var processor = new EventProcessorClient(
                storageClient,
                consumerGroup,
                eventHubsConnectionString,
                eventHubName);

            //var processor = new EventProcessorHost(eventHubName, consumerGroup, eventHubsConnectionString, storageConnectionString, blobContainerName);

            //await processor.RegisterEventProcessorAsync<SimpleEventProcessor1>();

            //processor.

            var cancellationSource = new CancellationTokenSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(30));


            processor.ProcessEventAsync += processEventHandler;
            processor.ProcessErrorAsync += processErrorHandler;

            await processor.StartProcessingAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30));
            }

            await processor.StopProcessingAsync(cancellationToken: stoppingToken);

           // return Task.CompletedTask;
            //throw new NotImplementedException();

        }

        protected abstract Task ProcessMessage(string messageId,string partition, string enqueuetime, IDictionary<string, object> userProperties, CancellationToken cancellationToken);
    }
}
