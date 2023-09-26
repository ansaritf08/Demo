namespace Publicis.ReportHub.Framework.DTO.MessageContracts
{
    public class DaprRootMessage<T>
    {
        public string Type { get; set; }

        public string Topic { get; set; }

        public string Source { get; set; }

        public string Specversion { get; set; }

        public string Datacontenttype { get; set; }

        public string Pubsubname { get; set; }

        public string Traceid { get; set; }

        public T Data { get; set; }

        public string Id { get; set; }
    }
}
