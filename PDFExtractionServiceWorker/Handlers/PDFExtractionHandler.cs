using DataAccess.Database;
using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using PDFExtractionLib.Handlers;
using Serilog;
using System.Text.Json;

namespace PDFExtractionServiceWorker.Handlers
{
    public class PDFExtractionHandler : IPDFExtractionHandler
    {
        private readonly Serilog.ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public PDFExtractionHandler(Serilog.ILogger logger, IConfiguration configuration, DataContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public Task<bool> HandelEvent(string message)
        {
            var pdfExtractionModel = JsonSerializer.Deserialize<TilsynsRapportToExtraxtModel>(message);
            return pdfExtractionModel is not null ? HandelPdfExtraction(pdfExtractionModel) : Task.FromResult(false);
        }

        public async Task<bool> HandelPdf(TilsynsRapportToExtraxtModel pdfExtractionModel)
        {
            return await HandelPdfExtraction(pdfExtractionModel);
        }

        private async Task<bool> HandelPdfExtraction(TilsynsRapportToExtraxtModel pdfExtractionModel)
        {
            try
            {
                //TODO: Add database access to DataAccess layer
              
                    // Perform database operations using this context

                IDownloadFile downLoadFile = new DownloadFileHandler(new HttpClient(), _logger);
                IPDFExtraction pdfExtractor = new PDFExtractionPDFText(_logger);

                if (_context.InstitutionReportCriterieaDatabasemodel.Any(x => x.ReportId == pdfExtractionModel.id)) return true;

                var filePath = Directory.GetCurrentDirectory() + _configuration.GetValue<string>("DownloadPath:folderPath");
                _logger.Information("File path: {filepath}", filePath);
                var filename = pdfExtractionModel.id.ToString() + "." + pdfExtractionModel.documentExtention;
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

                _context.InstitutionReportCriterieaDatabasemodel.Add(instRepCategories);

                _context.SaveChanges();

                    return true;
                
            }
            catch (Exception ex)
            {
                _logger.Error($"Error message: {ex.Message}\nInner Exception: {ex.InnerException}");
                return false;

            }
        }
    }



}
