
using System;


namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class TNTMessage
    {
        //public MessageHeader Message_Header { get; set; }
        //public StageInfo Stage_Info { get; set; }
        //public MessageBody Message_Body { get; set; }
        public FileReceived FileReceived { get; set; }
        public FileDebatched FileDebatched { get; set; }
        public MessageProcessed MessageProcessed { get; set; }
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
