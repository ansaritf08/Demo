using Microsoft.Extensions.DependencyInjection;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.Observability.Helpers;

namespace Publicis.ReportHub.Framework.Observability.DependencyResolvers
{
    public static class ObservabilityDependencyResolver
    {
        public static void ResolveObservabilityDependency(this IServiceCollection services, IObservabilitySettings observabilitySettings)
        {
            AppinsightsInitializer.SetupAppinsights(services, observabilitySettings);
        }
    }
}
