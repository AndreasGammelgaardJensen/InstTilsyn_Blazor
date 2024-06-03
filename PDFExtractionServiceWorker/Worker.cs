using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using PDFExtractionLib.Handlers;
using PDFExtractionLib.Tags;
using PDFExtractionServiceWorker.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using Serilog;
using static Org.BouncyCastle.Math.EC.ECCurve;
using DataAccess.Database;
using CoreInfrastructure.MessageBroker;

namespace PDFExtractionServiceWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IEventHandler _pdfHandler;
  
        private readonly ISubscriber _subscriber;

        public Worker(ILogger<Worker> logger, IPDFExtractionHandler pdfHandler, ISubscriber subscriber)
        {
            _logger = logger;
            _pdfHandler = pdfHandler;

            _subscriber = subscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriber.Subscribe(stoppingToken, _pdfHandler);

        }
    }
}