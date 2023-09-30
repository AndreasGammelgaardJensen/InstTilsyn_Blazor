// See https://aka.ms/new-console-template for more information
using VuggestueTilsynScraperLib.Scraping;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VuggestueTilsynScraper.Database;
using VuggestueTilsynScraper.Repositories;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using DataAccess.Interfaces;
using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using VuggestueTilsynScraper;

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
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    

    IConfigurationRoot configuration = builder.Build();

    Log.Information(configuration.GetConnectionString("SQLServer"));

    return Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(builder => { builder.AddConfiguration(configuration); })
        .ConfigureServices((_, services) =>
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.EnableSensitiveDataLogging(false);
                options.UseSqlServer(configuration.GetConnectionString("SQLServer"));

            });
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
}


static void SetupQueryableDatabaseModel<TDatabaseModel>(IServiceCollection services) where TDatabaseModel : class
{
    services.AddScoped(x => x.GetService<DataContext>().Get<TDatabaseModel>());
}


//using (var scope = host.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    try
//    {
//var context = services.GetRequiredService<DataContext>();
//context.Database.EnsureCreated();
//var config = services.GetRequiredService<IConfiguration>();


//try
//{
//Log.Debug<DataContext>("WTF", context);
//Log.Information("Bla");
//Log.Error("WTF");
//Log.Information(config.GetValue<string>("RabbitMQPDF:host"));
//ConnectionFactory factory = new();
//factory.Uri = new Uri(config.GetValue<string>("RabbitMQPDF:host"));
////factory.Uri = new Uri("amqp://guest:guest@some-rabbit_tilsyn");
//factory.ClientProvidedName = "Rabbit sender Report App";

//IConnection cnn = factory.CreateConnection();

//IModel channel = cnn.CreateModel();

//string exchangeName = config.GetValue<string>("RabbitMQPDF:exchangeName"); ;
//string routingKey = config.GetValue<string>("RabbitMQPDF:routingKey");
//string queueName = config.GetValue<string>("RabbitMQPDF:queueName"); ;

//channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
//channel.QueueDeclare(queueName, false, false, false, null);
//channel.QueueBind(queueName, exchangeName, routingKey, null);


//services.GetRequiredService<ReactScrapingHandler>().Handle((instId, list) => {

//    list.ForEach(x => {

//        var rmQ = new TilsynsRapportToExtraxtModel
//        {
//            id = x.Id,
//            downloadUrl = x.fileUrl,
//            institutionId = instId,
//        };

//        string messageString = JsonSerializer.Serialize(rmQ);

//        byte[] messagebody = Encoding.UTF8.GetBytes(messageString);
//        channel.BasicPublish(exchangeName, routingKey, null, messagebody);
//        Console.WriteLine("Message sent to rbmq");

//    });


//}, true);
//}
//catch (Exception e)
//{
//    Console.WriteLine(e);
//}
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex);
//    }
//}

host.RunAsync();





