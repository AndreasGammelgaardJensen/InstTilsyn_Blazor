using PDFExtractionServiceWorker;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using PDFExtractionServiceWorker.Database;
using PDFExtractionLib.Handlers;
using PDFExtractionServiceWorker.Handlers;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

IHost host = Host.CreateDefaultBuilder(args)

    
    .ConfigureServices(services =>
    {
        services.AddSingleton<DataContext>();
        services.AddSingleton<HttpClient>();
        services.AddSingleton<IPDFExtraction, PDFExtractionPDFText>();
        services.AddSingleton<IDownloadFile, DownloadFileHandler>();
        services.AddSingleton<IPDFExtractionHandler, PDFExtractionHandler>();
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

await host.RunAsync();
