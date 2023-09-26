using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
        public class ConstantsStageStatus
        {
            public const string Succeeded = "Succeeded";
            public const string InProgress = "InProgress";
            public const string InProcess = "InProcess";
            public const string Failed = "Failed";
            public const string Ignored = "Ignored";
            public const string Reprocess = "Reprocess";
            public const string NoMessageGenerated = "No Messages";
            public const string Suppressed = "Suppressed";
            public const string Scheduled = "Scheduled";
            public const string Rejected = "Rejected";
            public const string Accepted = "Accepted";
            public const string ACKReceived = "ACK Received";
            public const string Pending = "Pending";
            public const string PPending = "pPending";
            public const string PCancelled = "pCancelled";
            public const string Cancelled = "Cancelled";
            public const string Matched = "Matched";
            public const string Unmatched = "unMatched";
            public const string PMatched = "pMatched";
            public const string NA = "NA";
            public const string PendingForApproval = "Pending For Approval";
            public const string OnHold = "On Hold";
            public const string Warning = "Warning";
            public const string ToBeBatched = "To Be Batched";
            public const string HkmaNack = "HKMA NACK";
            public const string HkmaRejected = "HKMA Not Sent - New Trade Rejected";
            public const string HkmaNotActive = "HKMA Not Sent - Trade Already Not Active";
            public const string ISINOnHold = "ISIN On Hold";
            public const string Resolved = "Resolved";
            public const string Reprocessed = "Reprocessed";
            public const string Reprocessing = "Reprocessing";
            public const string OnHoldSEQN = "On Hold - SQN";
            public const string OnHoldNPODelay = "On Hold - NPO";
            public const string NACKReceived = "NACK Received";
        }
    
}
