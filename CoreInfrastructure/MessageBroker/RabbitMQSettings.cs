namespace CoreInfrastructure.MessageBroker
{
    public class RabbitMQSettings
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }
        public string HostUrl { get; set; }
    }
}