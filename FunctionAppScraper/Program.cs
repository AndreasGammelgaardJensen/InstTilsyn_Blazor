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
using VuggestueTilsynScraperLib.Scraping;
using FunctionAppScraper;
using CoreInfrastructure.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using FunctionAppScraper.FunctionHandlers;

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
    .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => {
        configurationBuilder.AddEnvironmentVariables();
        })

    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
//        services.AddDbContext<DataContext>(options =>
//        {
//            options.EnableSensitiveDataLogging(false);
//            options.UseSqlServer("Server=tcp:bgserverinst.database.windows.net,1433;Initial Catalog=inst-db-report;Persist Security Info=False;User ID=andreasbgjensen;Password=Firma2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
//);
//        });

       
        services.AddScoped<IEventHandler, PDFExtractionHandler>();
        services.AddSingleton(Log.Logger);
        services.AddLogging();
        services.AddScoped<ScrapingHandler>();
        services.AddScoped<IInstitutionRepository, InstitutionRepository>();
        services.AddScoped<ReactScrapingHandler>();
        

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

