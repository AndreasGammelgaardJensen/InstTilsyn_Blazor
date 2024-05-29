using Azure.Storage.Queues;
using Serilog;


namespace CoreInfrastructure.MessageBroker
{
    public class StorageQueueProvider : IPublisher
    {
        private readonly StorageProviderSettings _connectionString;
        private QueueClient _queue;
        private readonly ILogger _logger;

        public StorageQueueProvider(ILogger logger, StorageProviderSettings connectionString)
        {
            _logger = logger;
            _logger.Information("AzureQueue Connected ");
            _connectionString = connectionString;
            _queue = new QueueClient(_connectionString.ConnectionString, _connectionString.QueueName);
            _queue.Create();


        }

        public void PublishMessage(string message)
        {
            _queue.SendMessage(message);
            _logger.Debug($"Message sent: {message}");
        }
    }
}
