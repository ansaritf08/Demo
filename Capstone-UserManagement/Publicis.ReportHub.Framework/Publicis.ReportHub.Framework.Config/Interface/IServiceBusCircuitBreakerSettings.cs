namespace Publicis.ReportHub.Framework.Config.Interface
{
    public interface IServiceBusCircuitBreakerSettings
    {
        public int ExceptionsAllowedBeforeBreakingServiceBus { get; set; }

        public int DurationOfBreakInMinForServiceBus { get; set; }
    }
}
