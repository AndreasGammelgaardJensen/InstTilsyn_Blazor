using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using PDFExtractionLib.Handlers;
using PDFExtractionServiceWorker.Database;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

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
                    var filePath = Directory.GetCurrentDirectory() + _configuration.GetValue<string>("DownloadPath:folderPath");
                    var filename = pdfExtractionModel.id.ToString() + "." + pdfExtractionModel.documentExtention;


                    var reportDatabaseModel = await context.InstitutionReportCriterieaDatabasemodel.Include(x=>x.Categories).SingleOrDefaultAsync(x => x.ReportId == pdfExtractionModel.id);

                    if (reportDatabaseModel is not null && reportDatabaseModel.Categories.Any()) 
                    {
                        _logger.Information("Filereport and categoryies exist");
                        return true;
                    } else if (reportDatabaseModel is null)
                    {
                        _logger.Information("Add report");
                        _logger.Information("File path: {filepath}", filePath);
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
                    } else
                    {
                        _logger.Information("Updating Categories");

                        var categories = pdfExtractor.Extract_Categories(filePath + "/" + filename);
                        _logger.Information("Category count: {categories}", categories.Count());
                        foreach (var instRepCategory in categories)
                        {
                            reportDatabaseModel.Categories.Add(new CategoriClass
                            {
                                Id = Guid.NewGuid(),
                                CreatedAt = DateTime.Now,
                                LastChangedAt = DateTime.Now,
                                CategoriText = instRepCategory.Key,
                                Indsats = instRepCategory.Value
                            });
                        }
                    }

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
