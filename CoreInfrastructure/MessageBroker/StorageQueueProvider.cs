using Azure.Storage.Queues;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.MessageBroker
{
    public class StorageQueueProvider : IPublisher<string>
    {
        private readonly string _connectionString;
        private QueueClient _queue;
        private readonly ILogger _logger;

        public StorageQueueProvider(ILogger logger)
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _connectionString = connectionString;
            _queue = new QueueClient(_connectionString, "testqueue");
            _queue.Create();
            _logger = logger;
            _logger.Information("AzureQueue Connected ");
        }

        public void PublishMessage(string message)
        {
            _queue.SendMessage(message);
            _logger.Debug($"Message sent: {message}");
        }
    }
}
