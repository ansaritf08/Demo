using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class StageInfo
    {
        public string Message_Stage { get; set; }
        public string Message_Status { get; set; }
        public string Message_Next_Stage { get; set; }
        public string Message_Stage_Exception { get; set; }
        public string NotificationId { get; set; }
        public string CanProcessNotification { get; set; }
        public string NotificationType { get; set; }
        public string IsReprocessable { get; set; }
        public string Message_Next_Stage_Status { get; set; }
    }
}
