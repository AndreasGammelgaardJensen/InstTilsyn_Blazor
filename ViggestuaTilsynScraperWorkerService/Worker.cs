namespace ViggestuaTilsynScraperWorkerService
{
    public class Worker : IHostedService
    {
        public readonly IServiceProvider _services;
        private readonly ILogger<Worker> _logger;
        private Timer? _timer = null;

        public Worker(ILogger<Worker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {


            using (var scope = _services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    services.GetRequiredService<ReactScrapingHandler>().Handle((instId, list) => {

                        list.ForEach(x => {

                            var rmQ = new TilsynsRapportToExtraxtModel
                            {
                                id = x.Id,
                                downloadUrl = x.fileUrl,
                                institutionId = instId,
                            };

                            string messageString = JsonSerializer.Serialize(rmQ);

                            byte[] messagebody = Encoding.UTF8.GetBytes(messageString);
                            channel.BasicPublish(exchangeName, routingKey, null, messagebody);
                            Console.WriteLine("Message sent to rbmq");

                        });

                    }
                catch (Exception ex)
                {

                }
                _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        }




        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
