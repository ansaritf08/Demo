using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class FileTransformed
    {
       
        public string stageName  { get; set; }

        public string JobID { get; set; }
        public string SourceSystemName { get; set;}
        public string TenantID { get; set;}
        public string SourceType { get; set; }
        public string JobStatus { get; set; }

    }
}
