namespace Publicis.ReportHub.Framework.DTO.DataContracts
{
    public class DaprBaseCosmosRecord<T>
    {
        public string id { get; set; }

        public T value { get; set; }

        public bool isBinary { get; set; }

        public string partitionKey { get; set; }
    }
}
