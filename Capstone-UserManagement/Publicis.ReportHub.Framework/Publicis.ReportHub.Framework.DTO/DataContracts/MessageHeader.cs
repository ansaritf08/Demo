using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class MessageHeader
    {
        public string Message_Id { get; set; }
        public string Message_Ref_Id { get; set; }
        public string Message_Version_Number { get; set; }
        public string Message_Category { get; set; }
        public string Job_Id { get; set; }
        public string Batch_Id { get; set; }
        public int Batch_Item_Count { get; set; }
        public string Batch_Parent_Id { get; set; }
        public string As_Of_Date { get; set; }
        public string As_Of_Time { get; set; }
        public string Source_System_Name { get; set; }
        public string CancelMessage { get; set; }
    }
}
