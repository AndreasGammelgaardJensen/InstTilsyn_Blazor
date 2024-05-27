using DataAccess.Database;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelsLib.DatabaseModels;
using Serilog.Events;
using Serilog;
using VuggestueTilsynScraper;
using VuggestueTilsynScraperLib.Scraping;
using FunctionAppScraper;
using CoreInfrastructure.MessageBroker;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Fatal)

            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(options =>
        {
            options.EnableSensitiveDataLogging(false);

            if(Environment.GetEnvironmentVariable("DevelopmentMode") =="true")
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("SQLDockerConnectionString"));

            }
            else
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("SQLConnectionString"));
            }


        });

        if(Environment.GetEnvironmentVariable("MessageService") == "Azure")
        {
            services.AddTransient<IPublisher<string>, StorageQueueProvider>();

        }
        else
        {
            services.AddScoped<RabbitMQSettings>(x => new RabbitMQSettings
            {
                ExchangeName = Environment.GetEnvironmentVariable("RabbitMQPDFexchangeName"),
                RoutingKey = Environment.GetEnvironmentVariable("RabbitMQPDFroutingKey"),
                QueueName = Environment.GetEnvironmentVariable("RabbitMQPDFqueueName"),
                HostUrl = Environment.GetEnvironmentVariable("RabbitMQPDFhost")
            });
            services.AddTransient<IPublisher<string>, RabbitMQPublisher>();

        }


        services.AddTransient<ScrapingHandler>();
        services.AddScoped<IInstitutionRepository, InstitutionRepository>();
        services.AddScoped<ReactScrapingHandler>();
        services.AddSingleton(Log.Logger);

        SetupReadContext(services);
    })
    .Build();

host.Run();


static void SetupReadContext(IServiceCollection services)
{
    SetupQueryableDatabaseModel<InstitutionFrontPageModelDatabasemodel>(services);
    SetupQueryableDatabaseModel<InstitutionTilsynsRapportDatabasemodel>(services);
    SetupQueryableDatabaseModel<InstKoordinatesDatabasemodel>(services);
    SetupQueryableDatabaseModel<PladserDatabasemodel>(services);
    SetupQueryableDatabaseModel<AddressDatabasemodel>(services);
}

static void SetupQueryableDatabaseModel<TDatabaseModel>(IServiceCollection services) where TDatabaseModel : class
{
    services.AddScoped(x => x.GetService<DataContext>().Get<TDatabaseModel>());
}

