using System.Collections.Generic;

namespace Publicis.ReportHub.Framework.DTO.MessageContracts.Generator
{
    public class GeneratorEvent
    {
        public string Topic { get; set; }

        public string Subject { get; set; }

        public string EventType { get; set; }

        public string Id { get; set; }

        public GeneratorBlobData Data { get; set; }

        public string DataVersion { get; set; }

        public string MetadataVersion { get; set; }

        public string EventTime { get; set; }
    }

    public class GeneratorEvenListType
    {
        public List<GeneratorEvent> GeneratorEvenList { get; set; }
    }
}
