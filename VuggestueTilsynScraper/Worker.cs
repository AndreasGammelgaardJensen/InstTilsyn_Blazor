using CoreInfrastructure.MessageBroker;
using DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelsLib.Models.RabbitMQ;
using Serilog;
using System.Text.Json;
using VuggestueTilsynScraperLib.Scraping;

namespace VuggestueTilsynScraper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;
        private readonly IPublisher _messagePublisher;

        public Worker(ILogger logger, IConfiguration configuration, IServiceProvider services, IPublisher messagePublisher)
        {
            _logger = logger;
            _config = configuration;
            _services = services;
            _messagePublisher = messagePublisher;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
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

                    _logger.Information("KK SCRAPER Finished Scraping");

				}
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}
