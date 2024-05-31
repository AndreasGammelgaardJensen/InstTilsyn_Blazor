

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace CoreInfrastructure.MessageBroker
{
    public class RabbitMQSubscriber : ISubscriber
    {
        private readonly RabbitMQSettings _rabbitMQsettigns;
        private readonly ILogger _logger;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private readonly IConnection _cnn;

        public RabbitMQSubscriber(RabbitMQSettings rabbitMQsettigns, ILogger logger, IModel channel)
        {
            _rabbitMQsettigns = rabbitMQsettigns;
            _logger = logger;
            _channel = channel;

            _logger.Information("Subscribing on:");
            _logger.Information(_rabbitMQsettigns.HostUrl);
            _logger.Information(_rabbitMQsettigns.ExchangeName);
            _logger.Information(_rabbitMQsettigns.RoutingKey);
            _logger.Information(_rabbitMQsettigns.QueueName);

            ConnectionFactory factory = new();
            factory.Uri = new Uri(_rabbitMQsettigns.HostUrl);
            factory.ClientProvidedName = "Rabbit sender Report App";

            _cnn = factory.CreateConnection();
            _channel = _cnn.CreateModel();



            _channel.ExchangeDeclare(_rabbitMQsettigns.ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_rabbitMQsettigns.QueueName, false, false, false, null);
            _channel.QueueBind(_rabbitMQsettigns.QueueName, _rabbitMQsettigns.ExchangeName, _rabbitMQsettigns.RoutingKey, null);
            channel.BasicQos(0, 1, false);
            _consumer = new EventingBasicConsumer(channel);
        }

        public async void Subscribe(CancellationToken stoppingToken, IEventHandler eventHandler)
        {
            _consumer.Received += async (sender, args) =>
            {

                var body = args.Body.ToArray();

                string message = Encoding.UTF8.GetString(body);


                var pdfHandled = await eventHandler.HandelEvent(message);

                if (pdfHandled)
                    _channel.BasicAck(args.DeliveryTag, false);
            };

            string consumerTag = _channel.BasicConsume(_rabbitMQsettigns.QueueName, false, _consumer);


            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                //Add the Rabbit MQ

            }
            Console.ReadLine();
            _logger.Information("Closing connection");
            _channel.BasicCancel(consumerTag);
            _channel.Close();
            _cnn.Close();
            _logger.Information("Channel and connection closed");
        }
    }
}
