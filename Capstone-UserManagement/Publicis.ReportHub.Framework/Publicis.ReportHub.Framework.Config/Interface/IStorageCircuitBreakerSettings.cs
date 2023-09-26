namespace Publicis.ReportHub.Framework.Config.Interface
{
    public interface IStorageCircuitBreakerSettings
    {
        public int ExceptionsAllowedBeforeBreakingBlobStorage { get; set; }

        public int DurationOfBreakInMinForBlobStorage { get; set; }
    }
}
