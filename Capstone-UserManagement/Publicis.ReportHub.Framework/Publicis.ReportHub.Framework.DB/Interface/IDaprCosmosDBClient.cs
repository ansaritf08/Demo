using Publicis.ReportHub.Framework.DTO.DataContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.DB.Interface
{
    public interface IDaprCosmosDBClient
    {
        Task InsertReportHubOutputRecords(IEnumerable<ReportHubOutput> reportHubOutputs, string tenantName, string correlationId);

        Task UpsertReportHubOutputRecords(ReportHubOutput reportHubOutput, string tenantName);

        Task<TradeRegulatorVerificationLookup> FetchTradeRegulatorVerificationLookupRecord(string tenantName);
    }
}
