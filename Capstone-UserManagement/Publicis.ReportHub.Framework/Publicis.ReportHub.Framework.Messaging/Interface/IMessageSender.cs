using System.Threading.Tasks;

namespace Publicis.ReportHub.Framework.Messaging.Interface
{
    public interface IMessageSender
    {
        Task SendMessage<T>(string queueName, T message);

    }
}
