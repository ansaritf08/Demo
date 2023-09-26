using Publicis.ReportHub.Framework.Publicis.ReportHub.Framework.DTO.DataContracts;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Storage.Interface
{
    public interface IDaprStorageClient
    {
        Task InsertTRReportingOutput(TRReportingOutput TRReportingOutput, string tenantName);
    }
}
