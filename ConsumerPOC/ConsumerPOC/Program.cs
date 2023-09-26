using ConsumerPOC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            ServiceBusHostBuilder(args);
            IServiceCollection services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            //var _telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
            //_telemetryClient.Flush();
            //Task.Delay(5000).Wait();
        }
        
        catch (Exception exception)
        {
            var logMessage = string.Format(exception.Message);
            Console.WriteLine(logMessage);
            throw;
        }
    }

    private static void ServiceBusHostBuilder(string[] args)
    {
        Host.CreateDefaultBuilder(args)
           .UseWindowsService()
           .ConfigureHostConfiguration(configHost =>
           {
               configHost.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
           })
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHostedService<UserManagementServiceBus>();
               services.AddHostedService<UserManagementEventHub>();
           }).Build().Run();
    }
}