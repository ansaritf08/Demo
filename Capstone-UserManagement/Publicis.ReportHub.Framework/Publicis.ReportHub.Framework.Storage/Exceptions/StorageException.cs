using System;
using System.Runtime.Serialization;

namespace Publicis.ReportHub.Framework.Storage.Exceptions
{
    [Serializable]
    public class StorageException : Exception
    {
        public StorageException()
        {
        }

        public StorageException(string message) : base(message)
        {
        }

        public StorageException(string storageOperationName, Exception innerException)
            : base($"'{storageOperationName}' operation has failed with exception", innerException)
        {
        }

        protected StorageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
