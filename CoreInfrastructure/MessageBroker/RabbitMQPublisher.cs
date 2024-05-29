using RabbitMQ.Client;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CoreInfrastructure.MessageBroker
{
    public class RabbitMQPublisher : IPublisher
    {
        private readonly RabbitMQSettings _rabbitMQsettigns;
        private readonly ILogger _logger;
        private IModel _channel;

        public RabbitMQPublisher(RabbitMQSettings rabbitMQsettigns, ILogger logger)
        {
            _rabbitMQsettigns = rabbitMQsettigns;
            _logger = logger;

            _logger.Information(_rabbitMQsettigns.HostUrl);
            _logger.Information(_rabbitMQsettigns.ExchangeName);
            _logger.Information(_rabbitMQsettigns.RoutingKey);
            _logger.Information(_rabbitMQsettigns.QueueName);

            ConnectionFactory factory = new();
            factory.Uri = new Uri(_rabbitMQsettigns.HostUrl);
            factory.ClientProvidedName = "Rabbit sender Report App";

            IConnection cnn = factory.CreateConnection();

            _channel = cnn.CreateModel();



            _channel.ExchangeDeclare(_rabbitMQsettigns.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_rabbitMQsettigns.QueueName, false, false, false, null);
            _channel.QueueBind(_rabbitMQsettigns.QueueName, _rabbitMQsettigns.ExchangeName, _rabbitMQsettigns.RoutingKey, null);

        }

        public void PublishMessage(string message)
        {
            byte[] messagebody = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(_rabbitMQsettigns.ExchangeName, _rabbitMQsettigns.RoutingKey, null, messagebody);
            _logger.Information($"Message sent: \"Message sent to rbmq\"");
        }
    }
}
