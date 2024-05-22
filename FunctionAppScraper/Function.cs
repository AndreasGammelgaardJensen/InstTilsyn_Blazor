using System;
using Microsoft.Azure.Functions.Worker;
using Serilog;
using VuggestueTilsynScraper;

namespace FunctionAppScraper
{
    public class Function
    {
        private readonly ILogger _logger;
        private readonly ScrapingHandler _worker;

        public Function(ILogger loggerFactory, ScrapingHandler worker)
        {
            _logger = loggerFactory;
            _worker = worker;
        }

        [Function("Function")]
        public void Run([TimerTrigger("00:00:10")] TimerInfo myTimer)
        {
            _logger.Information($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.Information($"Next timer schedule at: {Environment.GetEnvironmentVariable("SQLConnectionString")}");

            _worker.Execute(new CancellationToken());

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.Information($"Next timer schedule at: {Environment.GetEnvironmentVariable("SQLConnectionString")}");
            }
        }
    }
}
