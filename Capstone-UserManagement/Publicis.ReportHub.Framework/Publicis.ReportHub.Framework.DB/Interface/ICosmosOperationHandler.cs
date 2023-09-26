using Publicis.ReportHub.Framework.DTO.DataContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.DB.Interface
{
    public interface ICosmosOperationHandler
    {
        Task<ReportHubOutput> FetchReportHubOutput(string containerName, string CosmosRecordId, string correlationId);
    }
}
