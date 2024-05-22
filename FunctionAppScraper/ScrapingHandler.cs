

using DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelsLib.Models.RabbitMQ;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;
using VuggestueTilsynScraperLib.Scraping;

namespace FunctionAppScraper
{
    public class ScrapingHandler
    {

        private readonly ILogger _logger;
        private readonly IServiceProvider _services;

        public ScrapingHandler(ILogger logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }
        public void Execute(CancellationToken stoppingToken)
        {

            string exchangeName = Environment.GetEnvironmentVariable("RabbitMQPDFexchangeName");
            string routingKey = Environment.GetEnvironmentVariable("RabbitMQPDFroutingKey");
            string queueName = Environment.GetEnvironmentVariable("RabbitMQPDFqueueName");
            string hostUrl = Environment.GetEnvironmentVariable("RabbitMQPDFhost");


            _logger.Information(hostUrl);
            _logger.Information(exchangeName);
            _logger.Information(routingKey);
            _logger.Information(queueName);

            try
            {
                ConnectionFactory factory = new();
                factory.Uri = new Uri(hostUrl);
                factory.ClientProvidedName = "Rabbit sender Report App";

                IConnection cnn = factory.CreateConnection();

                IModel channel = cnn.CreateModel();

                

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
                        }, true);

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
