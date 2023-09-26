namespace Publicis.ReportHub.Framework.Config.Interface
{
    public interface IObservabilitySettings
    {
        string AppInsightInstrumentationKey { get; set; }

        string LogLevel { get; set; }
    }
}
