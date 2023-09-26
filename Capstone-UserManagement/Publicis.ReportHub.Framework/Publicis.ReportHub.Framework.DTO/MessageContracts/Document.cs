using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.DTO.MessageContracts
{
    public class Document
    {
        public string id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
    }
}
