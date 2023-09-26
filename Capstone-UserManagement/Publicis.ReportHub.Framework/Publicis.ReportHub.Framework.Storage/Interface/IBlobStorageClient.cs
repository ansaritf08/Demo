using System.IO;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Storage.Interface
{
    public interface IBlobStorageClient
    {
        Task<Stream> ReadBlobContent(string blobURL);

        string BlobName { get; set; }
    }
}
