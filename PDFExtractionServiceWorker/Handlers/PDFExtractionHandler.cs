using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using PDFExtractionLib.Handlers;
using PDFExtractionServiceWorker.Database;
using Serilog;

namespace PDFExtractionServiceWorker.Handlers
{
    public class PDFExtractionHandler : IPDFExtractionHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public PDFExtractionHandler(Serilog.ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<bool> HandelPdf(TilsynsRapportToExtraxtModel pdfExtractionModel)
        {
            try
            {
                //TODO: Add database access to DataAccess layer
                using (var context = new DataContext(_configuration))
                {
                    // Perform database operations using this context

                    IDownloadFile downLoadFile = new DownloadFileHandler(new HttpClient(), _logger);
                    IPDFExtraction pdfExtractor = new PDFExtractionPDFText(_logger);

                    if(context.InstitutionReportCriterieaDatabasemodel.Any(x => x.ReportId == pdfExtractionModel.id)) return true;

                    var filePath = Directory.GetCurrentDirectory() + _configuration.GetValue<string>("DownloadPath:folderPath");
                    _logger.Information("File path: {filepath}", filePath);
                    var filename = pdfExtractionModel.id.ToString() +"."+ pdfExtractionModel.documentExtention;
                    await downLoadFile.DownloadFile(filePath, filename, pdfExtractionModel.downloadUrl);
                    var test = pdfExtractor.Extract_Categories(filePath + "/" + filename);

                    var instRepCategories = new InstitutionReportCriterieaDatabasemodel
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        LastChangedAt = DateTime.Now,
                        InstitutionId = pdfExtractionModel.institutionId,
                        ReportId = pdfExtractionModel.id,
                        Categories = new List<CategoriClass>(),
                        fileUrl = pdfExtractionModel.downloadUrl,
                    };

                    foreach (var instRepCategory in test)
                    {
                        instRepCategories.Categories.Add(new CategoriClass
                        {
                            Id = Guid.NewGuid(),
                            CreatedAt = DateTime.Now,
                            LastChangedAt = DateTime.Now,
                            CategoriText = instRepCategory.Key,
                            Indsats = instRepCategory.Value
                        });
                    }

                    context.InstitutionReportCriterieaDatabasemodel.Add(instRepCategories);

                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error message: {ex.Message}\nInner Exception: {ex.InnerException}" );
                return false;

            }
        }
    }
}
