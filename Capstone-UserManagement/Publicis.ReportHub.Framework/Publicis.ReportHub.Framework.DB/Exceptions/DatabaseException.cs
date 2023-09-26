using System;
using System.Runtime.Serialization;

namespace Publicis.ReportHub.Framework.DB.Exceptions
{
    [Serializable]
    public class DatabaseException : Exception
    {
        public DatabaseException()
        {
        }

        public DatabaseException(string message) : base(message)
        {
        }

        public DatabaseException(string DBOperationName, Exception innerException)
            : base($"'{DBOperationName}' operation has failed with exception", innerException)
        {
        }

        protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
