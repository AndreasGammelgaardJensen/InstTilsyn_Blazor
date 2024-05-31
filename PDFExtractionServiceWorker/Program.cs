using PDFExtractionServiceWorker;
using PDFExtractionLib.Handlers;
using PDFExtractionServiceWorker.Handlers;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using ModelsLib.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using DataAccess.Database;
using CoreInfrastructure.MessageBroker;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


IConfigurationRoot configuration = builder.Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(builder => { builder.AddConfiguration(configuration); })
    .ConfigureServices(services =>
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.EnableSensitiveDataLogging(false);
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));

        });

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
        services.AddSingleton<HttpClient>();
        services.AddScoped<IPDFExtraction, PDFExtractionPDFText>();
        services.AddScoped<IDownloadFile, DownloadFileHandler>();
        services.AddScoped<IPDFExtractionHandler, PDFExtractionHandler>();
        services.AddScoped<ISubscriber, RabbitMQSubscriber>();

        services.AddHostedService<Worker>();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Fatal)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
        SetupReadContext(services);


    })
    .Build();


using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.EnsureCreated();

    }catch (Exception ex)
    {

    }
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


await host.RunAsync();
