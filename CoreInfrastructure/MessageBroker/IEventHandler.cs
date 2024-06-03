using ModelsLib.Models.RabbitMQ;

namespace CoreInfrastructure.MessageBroker
{
    public interface IEventHandler
    {
        public Task<bool> HandelEvent(string message);
    }
}