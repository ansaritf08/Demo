using System;
using System.Runtime.Serialization;

namespace Publicis.ReportHub.Framework.Messaging.Exceptions
{
    [Serializable]
    public class MessageFrameworkException : Exception
    {
        public MessageFrameworkException()
        {
        }

        public MessageFrameworkException(string message) : base(message)
        {
        }

        public MessageFrameworkException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MessageFrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MessageFrameworkException(string opeartiontype, string messageType, string message,  Exception innerException)
            : base($"Message of type {messageType} failed to {opeartiontype}. Message:{message}", innerException)
        {
        }

    }
}
