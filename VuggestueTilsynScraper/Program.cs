// See https://aka.ms/new-console-template for more information
using VuggestueTilsynScraperLib.Scraping;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DataAccess.Interfaces;
using ModelsLib.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using VuggestueTilsynScraper;
using DataAccess.Repositories;
using DataAccess.Database;
using CoreInfrastructure.MessageBroker;

using IHost host = CreateHostBuilder(args).Build();

static IHostBuilder CreateHostBuilder(string[] args)
{

    Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Fatal)

            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();


    var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
    

    IConfigurationRoot configuration = builder.Build();

    Log.Information(configuration.GetConnectionString("SQLConnectionString"));

    return Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(builder => { builder.AddConfiguration(configuration); })
        .ConfigureServices((_, services) =>
        {
            services.AddDbContextFactory<DataContext>(options =>
            {
                options.EnableSensitiveDataLogging(false);
                options.UseSqlServer(configuration.GetConnectionString("SQLConnectionString"));

            });


            string messageProvider = configuration.GetValue<string>("MessageService");
            if (messageProvider == "Azure")
            {
                services.AddScoped<StorageProviderSettings>(x => new StorageProviderSettings
                {
                    ConnectionString = configuration.GetValue<string>("AzureWebJobsStorage"),
                    QueueName= configuration.GetValue<string>("QueueName")
,
                });

                services.AddScoped<IPublisher, StorageQueueProvider>();
            }
            else
            {
                string exchangeName = configuration.GetValue<string>("RabbitMQPDF:exchangeName");
                string routingKey = configuration.GetValue<string>("RabbitMQPDF:routingKey");
                string queueName = configuration.GetValue<string>("RabbitMQPDF:queueName");
                string hostUrl = configuration.GetValue<string>("RabbitMQPDF:host");
                services.AddScoped<RabbitMQSettings>(x => new RabbitMQSettings
                {
                    ExchangeName = exchangeName,
                    RoutingKey = routingKey,
                    QueueName = queueName,
                    HostUrl = hostUrl
                });
                services.AddScoped<IPublisher, RabbitMQPublisher>();
            }

            services.AddHostedService<Worker>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<ReactScrapingHandler>();
            services.AddSingleton(Log.Logger);
            SetupReadContext(services);

        });
}

static void SetupReadContext(IServiceCollection services)
{
    SetupQueryableDatabaseModel<InstitutionFrontPageModelDatabasemodel>(services);
    SetupQueryableDatabaseModel<InstitutionTilsynsRapportDatabasemodel>(services);
    SetupQueryableDatabaseModel<InstKoordinatesDatabasemodel>(services);
    SetupQueryableDatabaseModel<PladserDatabasemodel>(services);
    SetupQueryableDatabaseModel<AddressDatabasemodel>(services);
    SetupQueryableDatabaseModel<InstitutionReportCriterieaDatabasemodel>(services);
    SetupQueryableDatabaseModel<CategoriClass>(services);
}


static void SetupQueryableDatabaseModel<TDatabaseModel>(IServiceCollection services) where TDatabaseModel : class
{
    services.AddScoped(x => x.GetService<DataContext>().Get<TDatabaseModel>());
}

host.RunAsync();





