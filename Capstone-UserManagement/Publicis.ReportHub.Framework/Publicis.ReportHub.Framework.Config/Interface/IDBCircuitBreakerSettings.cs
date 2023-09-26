namespace Publicis.ReportHub.Framework.Config.Interface
{
    public interface IDBCircuitBreakerSettings
    {
        public int ExceptionsAllowedBeforeBreakingCosmosDB { get; set; }

        public int DurationOfBreakInMinForCosmosDB { get; set; }
    }
}
