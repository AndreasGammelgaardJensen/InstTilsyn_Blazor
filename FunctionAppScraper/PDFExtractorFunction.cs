using System;
using Azure.Storage.Queues.Models;
using CoreInfrastructure.MessageBroker;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppScraper
{
    public class PDFExtractorFunction
    {
        private readonly ILogger<PDFExtractorFunction> _logger;
        private readonly IEventHandler _eventHandler;

        public PDFExtractorFunction(ILogger<PDFExtractorFunction> logger, IEventHandler eventHandler)
        {
            _logger = logger;
            _eventHandler = eventHandler;
        }

        [Function(nameof(PDFExtractorFunction))]
        public void Run([QueueTrigger("testqueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            _eventHandler.HandelEvent(message.MessageText);
        }
    }
}
