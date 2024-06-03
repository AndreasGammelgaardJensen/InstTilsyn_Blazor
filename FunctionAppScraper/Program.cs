using CoreInfrastructure.MessageBroker;
using CoreInfrastructure.Services.BlobServices;
using DataAccess.Database;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using FunctionAppScraper;
using FunctionAppScraper.FunctionHandlers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelsLib.DatabaseModels;
using Serilog;
using Serilog.Events;
using VuggestueTilsynScraperLib.Scraping;

var filepath = string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("WEBSITE_CONTENTSHARE")) ?
                        "log.txt" :
                        @"D:\home\LogFiles\Application\log.txt";

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Fatal)

            .Enrich.FromLogContext()
            .WriteTo.Console(LogEventLevel.Debug)
            .WriteTo.File(filepath, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => {
        configurationBuilder.AddEnvironmentVariables();
        })

    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

       
        services.AddScoped<IEventHandler, PDFExtractionHandler>();
        services.AddSingleton(Log.Logger);
        services.AddScoped<ScrapingHandler>();
        services.AddScoped<IInstitutionRepository, InstitutionRepository>();
        services.AddScoped<ReactScrapingHandler>();
        services.AddSingleton<IBlobService, BlobService>();

        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), preferMsi: true);
        });

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
    SetupQueryableDatabaseModel<InstitutionReportCriterieaDatabasemodel>(services);
    SetupQueryableDatabaseModel<CategoriClass>(services);
}

static void SetupQueryableDatabaseModel<TDatabaseModel>(IServiceCollection services) where TDatabaseModel : class
{
    services.AddScoped(x => x.GetService<DataContext>().Get<TDatabaseModel>());
}

