using System;
using System.Collections.Generic;
using System.Text;

namespace Publicis.ReportHub.Framework.Config.Interface
{ 
    public interface IEventHubSettings
    {
         string ehubNamespaceConnectionString { get; set; }
        string eventHubName { get; set; }
        string blobStorageConnectionString { get; set; }
        string blobContainerName { get; set; }

      //public string[] ehubNamespaceConnectionStringList { get; set; }


    }


}
