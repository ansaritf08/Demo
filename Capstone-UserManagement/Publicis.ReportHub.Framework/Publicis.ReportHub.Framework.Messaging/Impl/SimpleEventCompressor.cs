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
using ZipHelperLib;

namespace Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.Messaging.Impl
{
    public abstract class SimpleEventCompressor : BackgroundService
    {
        string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=apmstestapppoc;AccountKey=+A5EH8OUHkRY5aWrXagw1Y3sLQd/+BlhrxThiGVbKyHcXoEhK0jDzG6z1XIz/QbcJD0KvDZEgRPu/w20MS/uVg==;EndpointSuffix=core.windows.net";
        string blobContainerName = "evenhubcontainernew";

        string eventHubsConnectionString = "Endpoint=sb://messageprocessing.servicebus.windows.net/;SharedAccessKeyName=listeninboundmsg;SharedAccessKey=kb1WOpgJVESNVprgamQLQc8RRnDgzHUN7TJ8gUblLdM=;EntityPath=inboundtransformationeh";
        string eventHubName = "inboundtransformationeh";
        string consumerGroup = "$Default";

        private static bool DefaultUseGZip = true;
        private static bool UseGZip { get; set; } = DefaultUseGZip;

        public async Task processEventHandler(ProcessEventArgs args)
        {          
            try
            {
                
                var partitionEventCount = new ConcurrentDictionary<string, int>();

                if (args.CancellationToken.IsCancellationRequested)
                {
                    return;
                }
                var content = args.Data.Properties.ContainsKey("ContentEncoding") ?args.Data?.Properties["ContentEncoding"]: null;
                //bool UseGZip = true;
                if (content!=null)
                {
                    if (content.Equals("gzip") )
                    {
                        UseGZip = true;
                    }
                    else
                    {
                        UseGZip = false;
                    }
                    Console.WriteLine($"UseGZip changed to {UseGZip}");
                    //reportedProperties["useGZip"] = UseGZip;
                }
                else
                {
                    Console.WriteLine($"UseGZip ignored");
                }

                var body = args.Data.EventBody.ToArray();
                var zippedMessageBytes = content==null? body :UseGZip
                                ? GZipHelper.Unzip(body)
                                : DeflateHelper.Unzip(body);

                var EnqueuedTime = args.Data.EnqueuedTime.ToString();
                string partition = args.Partition.PartitionId;
                
                
                byte[] eventBody = args.Data.EventBody.ToArray();
                Debug.WriteLine($"Event from partition { partition } with length { eventBody.Length }.");

                await ProcessMessage(Encoding.UTF8.GetString(zippedMessageBytes), partition, EnqueuedTime,
                       args.Data.Properties, 
                       args.CancellationToken);


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

            //var cancellationSource = new CancellationTokenSource();
            //cancellationSource.CancelAfter(TimeSpan.FromSeconds(30));


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
