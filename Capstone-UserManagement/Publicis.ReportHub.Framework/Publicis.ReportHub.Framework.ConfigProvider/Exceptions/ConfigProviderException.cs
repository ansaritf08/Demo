using System;
using System.Runtime.Serialization;

namespace Publicis.ReportHub.Framework.ConfigProvider.Exceptions
{
    [Serializable]
    public class ConfigProviderException : Exception
    {
        public ConfigProviderException()
        {
        }

        public ConfigProviderException(string message) : base(message)
        {
        }

        public ConfigProviderException(string configKey, Exception innerException) 
            : base($"Failed to provide config for key '{configKey}'", innerException)
        {
        }

        protected ConfigProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
