using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.Storage.Interface;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Storage.Impl
{
    public class BlobStorageClient : IBlobStorageClient
    {
        private readonly IStorageSettings _storageSettings;

        public BlobStorageClient(IStorageSettings storageSettings)
        {
            _storageSettings = storageSettings;
        }

        public string BlobName { get; set; }

        public async Task<Stream> ReadBlobContent(string blobURL)
        {
            var storageCred = new StorageCredentials(_storageSettings.StorageName, _storageSettings.StorageAccountKey);

            CloudBlockBlob cloudBlockBlob = new CloudBlockBlob(new Uri(blobURL), storageCred);

            string blobContent = await cloudBlockBlob.DownloadTextAsync();

            BlobName = cloudBlockBlob.Name;

            byte[] byteArray = Encoding.ASCII.GetBytes(blobContent);
            return new MemoryStream(byteArray);
        }
    }
}
