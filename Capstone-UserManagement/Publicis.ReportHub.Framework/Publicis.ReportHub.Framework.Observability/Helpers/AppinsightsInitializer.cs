using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Publicis.ReportHub.Framework.Config.Interface;
using System;

namespace Publicis.ReportHub.Framework.Observability.Helpers
{
    public static class AppinsightsInitializer
    {
        public static void SetupAppinsights(IServiceCollection services, IObservabilitySettings observabilitySettings)
        {
            if (!Enum.TryParse(observabilitySettings.LogLevel, true, out LogLevel logLevel) || !Enum.IsDefined(typeof(LogLevel), logLevel))
            {
                logLevel = LogLevel.Information;
            }

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddFilter<ApplicationInsightsLoggerProvider>("", logLevel);
            });

            services.AddApplicationInsightsTelemetryWorkerService(observabilitySettings.AppInsightInstrumentationKey);
        }
    }
}
