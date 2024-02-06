

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelsLib.Models.RabbitMQ;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;
using VuggestueTilsynScraper.Database;
using VuggestueTilsynScraperLib.Scraping;

namespace VuggestueTilsynScraper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;

        public Worker(ILogger logger, IConfiguration configuration, IServiceProvider services)
        {
            _logger = logger;
            _config = configuration;
            _services = services;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information(_config.GetValue<string>("RabbitMQPDF:host"));
            _logger.Information(_config.GetValue<string>("RabbitMQPDF:exchangeName"));
            _logger.Information(_config.GetValue<string>("RabbitMQPDF:routingKey"));
            _logger.Information(_config.GetValue<string>("RabbitMQPDF:queueName"));

            try
            {
                ConnectionFactory factory = new();
                factory.Uri = new Uri(_config.GetValue<string>("RabbitMQPDF:host"));
                factory.ClientProvidedName = "Rabbit sender Report App";

                IConnection cnn = factory.CreateConnection();

                IModel channel = cnn.CreateModel();

                string exchangeName = _config.GetValue<string>("RabbitMQPDF:exchangeName");
                string routingKey = _config.GetValue<string>("RabbitMQPDF:routingKey");
                string queueName = _config.GetValue<string>("RabbitMQPDF:queueName");

                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.QueueBind(queueName, exchangeName, routingKey, null);

                using (var scope = _services.CreateScope())
                {

                    var context = _services.GetRequiredService<DataContext>();
                    context.Database.EnsureCreated();

                    var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<ReactScrapingHandler>();
                    do
                    {
                        scopedProcessingService.Handle((instId, list) =>
                        {

                            list.ForEach(x =>
                            {
                                var rmQ = new TilsynsRapportToExtraxtModel
                                {
                                    id = x.Id,
                                    downloadUrl = x.fileUrl,
                                    institutionId = instId,
                                    documentExtention = x.documentType
                                };

                                string messageString = JsonSerializer.Serialize(rmQ);

                                byte[] messagebody = Encoding.UTF8.GetBytes(messageString);
                                channel.BasicPublish(exchangeName, routingKey, null, messagebody);
                                Log.Information("Message sent to rbmq");
                            });
                        },true);

                        Thread.Sleep(86400);
                    } while (!stoppingToken.IsCancellationRequested);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}
