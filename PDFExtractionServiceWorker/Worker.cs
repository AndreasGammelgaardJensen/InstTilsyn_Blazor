using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using PDFExtractionLib.Handlers;
using PDFExtractionLib.Tags;
using PDFExtractionServiceWorker.Database;
using PDFExtractionServiceWorker.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using Serilog;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace PDFExtractionServiceWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPDFExtraction _pdfExtractor;
        private readonly HttpClient _httpClient;
        private readonly IDownloadFile _downLoadFile;
        private readonly DataContext _dataContext;
        private readonly IPDFExtractionHandler _pdfHandler;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IPDFExtraction pdfExtractor, HttpClient httpClient, IDownloadFile downLoadFile, DataContext dataContext, IPDFExtractionHandler pdfHandler, IConfiguration configuration)
        {
            _logger = logger;
            _pdfExtractor = pdfExtractor;
            _httpClient = httpClient;
            _downLoadFile = downLoadFile;
            _dataContext = dataContext;
            _pdfHandler = pdfHandler;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(_configuration.GetValue<string>("RabbitMQPDF:host"));
            factory.ClientProvidedName = "Rabbit sender Report App";
            IConnection cnn = factory.CreateConnection();
            IModel channel = cnn.CreateModel();

            string exchangeName = _configuration.GetValue<string>("RabbitMQPDF:exchangeName"); ;
            string routingKey = _configuration.GetValue<string>("RabbitMQPDF:routingKey");
            string queueName = _configuration.GetValue<string>("RabbitMQPDF:queueName"); ;

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);
            //0 means that we do not care about the message size
            //1 means that we only want it to send one massage at the time
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {

                var body = args.Body.ToArray();

                string message = Encoding.UTF8.GetString(body);

                var extractionObject = JsonSerializer.Deserialize<TilsynsRapportToExtraxtModel>(message);


                var pdfHandled = await _pdfHandler.HandelPdf(extractionObject);

                if(pdfHandled)
                    channel.BasicAck(args.DeliveryTag, false);



                //try
                //{
                //    using (var context = new DataContext())
                //    {
                //        // Perform database operations using this context

                //        IDownloadFile downLoadFile = new DownloadFileHandler(new HttpClient());
                //        IPDFExtraction pdfExtractor = new PDFExtractionPDFText();
                //        var body = args.Body.ToArray();

                //        string message = Encoding.UTF8.GetString(body);

                //        var extractionObject = JsonSerializer.Deserialize<TilsynsRapportToExtraxtModel>(message);
                //        Console.WriteLine(message);

                //        //If it did not work, we do not send back an ack

                //        var filePath = "C:/Users/andreas.jensen/source/repos/TilsynVuggestueBlazer/VuggestueTilsyn/PDFExtractionLib";
                //        var filename = extractionObject.id.ToString() + ".pdf";
                //        downLoadFile.DownloadFile(filePath, filename, extractionObject.downloadUrl).GetAwaiter().GetResult();
                //        var test = pdfExtractor.Extract_Categories(filePath + "/" + filename);

                //        var instRepCategories = new InstitutionReportCriterieaDatabasemodel
                //        {
                //            Id = Guid.NewGuid(),
                //            InstitutionId = extractionObject.institutionId,
                //            ReportId = extractionObject.id,
                //            Categories = new List<CategoriClass>()
                //        };

                //        foreach (var instRepCategory in test)
                //        {
                //            instRepCategories.Categories.Add(new CategoriClass
                //            {
                //                Id = Guid.NewGuid(),
                //                CategoriText = instRepCategory.Key,
                //                Indsats = instRepCategory.Value
                //            });
                //        }

                //        context.InstitutionReportCriterieaDatabasemodel.Add(instRepCategories);

                //        context.SaveChanges();

                //        channel.BasicAck(args.DeliveryTag, false);

                //    }

                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Error", ex.Message);

                //}




                //Thread thread = new Thread(async () =>
                //{
                //    using (var context = new DataContext())
                //    {
                //        // Perform database operations using this context

                //        IDownloadFile downLoadFile = new DownloadFileHandler(new HttpClient());
                //        IPDFExtraction pdfExtractor = new PDFExtractionHandler();
                //        var body = args.Body.ToArray();

                //        string message = Encoding.UTF8.GetString(body);

                //        var extractionObject = JsonSerializer.Deserialize<TilsynsRapportToExtraxtModel>(message);
                //        Console.WriteLine(message);

                //        //If it did not work, we do not send back an ack

                //        var filePath = "C:/Users/andreas.jensen/source/repos/TilsynVuggestueBlazer/VuggestueTilsyn/PDFExtractionLib";
                //        var filename = extractionObject.id.ToString() + ".pdf";
                //        await downLoadFile.DownloadFile(filePath, filename, extractionObject.downloadUrl);
                //        var test = pdfExtractor.Extract_Categories(filePath + "/" + filename);

                //        var instRepCategories = new InstitutionReportCriterieaDatabasemodel
                //        {
                //            Id = Guid.NewGuid(),
                //            InstitutionId = extractionObject.institutionId,
                //            ReportId = extractionObject.id,
                //            Categories = new List<CategoriClass>()
                //        };

                //        foreach (var instRepCategory in test)
                //        {
                //            instRepCategories.Categories.Add(new CategoriClass
                //            {
                //                Id = Guid.NewGuid(),
                //                CategoriText = instRepCategory.Key,
                //                Indsats = instRepCategory.Value
                //            });
                //        }

                //        context.InstitutionReportCriterieaDatabasemodel.Add(instRepCategories);

                //        context.SaveChanges();

                //        channel.BasicAck(args.DeliveryTag, false);



                //    }
                //});
                //thread.Start();





            };

            string consumerTag = channel.BasicConsume(queueName, false, consumer);


            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                //Add the Rabbit MQ

            }
            Console.ReadLine();
            _logger.LogInformation("Closing connection");
            channel.BasicCancel(consumerTag);
            channel.Close();
            cnn.Close();
            _logger.LogInformation("Channel and connection closed");

        }

        private async Task ScrabeFunction(IModel channel, BasicDeliverEventArgs args, IPDFExtraction pdfExtractor, HttpClient httpClient, IDownloadFile downLoadFile, DataContext dataContext)
        {
            var body = args.Body.ToArray();

            string message = Encoding.UTF8.GetString(body);

            var extractionObject = JsonSerializer.Deserialize<TilsynsRapportToExtraxtModel>(message);
            Console.WriteLine(message);

            //If it did not work, we do not send back an ack
            //TODO: Add this path to appsettings
            var filePath = Directory.GetCurrentDirectory() + _configuration.GetValue<string>("DownloadPath:folderPath");
            
            var filename = extractionObject.id.ToString() + ".pdf";
            await _downLoadFile.DownloadFile(filePath, filename, extractionObject.downloadUrl);
            var test = _pdfExtractor.Extract_Categories(filePath + "/" + filename);

            var instRepCategories = new InstitutionReportCriterieaDatabasemodel
            {
                Id = Guid.NewGuid(),
                InstitutionId = extractionObject.institutionId,
                ReportId = extractionObject.id,
                Categories = new List<CategoriClass>()
            };

            foreach (var instRepCategory in test)
            {
                instRepCategories.Categories.Add(new CategoriClass
                {
                    Id = Guid.NewGuid(),
                    CategoriText = instRepCategory.Key,
                    Indsats = instRepCategory.Value
                });
            }

            _dataContext.Add(instRepCategories);

            _dataContext.SaveChanges();

            channel.BasicAck(args.DeliveryTag, false);
        }
    }

    
}