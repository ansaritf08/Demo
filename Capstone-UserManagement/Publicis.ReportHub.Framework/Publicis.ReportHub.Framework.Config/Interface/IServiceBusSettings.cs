namespace Publicis.ReportHub.Framework.Config.Interface
{
    public interface IServiceBusSettings
    {
        string ServiceBusConnectionString { get; set; }

         string ServiceBusMessageGeneratorQueueName { get; set; }

         string ServiceBusMessageMonikerQueueName { get; set; }

        string ServiceBusEventGridQueueName { get; set; }
    }
}
