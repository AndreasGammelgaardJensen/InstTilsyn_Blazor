

using CoreInfrastructure.MessageBroker;
using DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelsLib.Models.RabbitMQ;
using Serilog;
using System.Text.Json;
using VuggestueTilsynScraperLib.Scraping;

namespace FunctionAppScraper
{
    public class ScrapingHandler
    {

        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly IPublisher _messagePublisher;

        public ScrapingHandler(ILogger logger, IServiceProvider services, IPublisher messagePublisher)
        {
            _logger = logger;
            _services = services;
            _messagePublisher = messagePublisher;
        }
        public void Execute(CancellationToken stoppingToken)
        {

            try
            {
                
                using (var scope = _services.CreateScope())
                {

                    var context = _services.GetRequiredService<DataContext>();
                    context.Database.EnsureCreated();

                    var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<ReactScrapingHandler>();
                   
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
                            _messagePublisher.PublishMessage(messageString);
                        });
                    }, true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
    
}
