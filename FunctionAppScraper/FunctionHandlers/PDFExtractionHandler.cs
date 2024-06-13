using Azure;
using CoreInfrastructure.MessageBroker;
using CoreInfrastructure.Services.BlobServices;
using DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using ModelsLib.Models.RabbitMQ;
using PDFExtractionLib.Handlers;
using Serilog;
using System.Text.Json;

namespace FunctionAppScraper.FunctionHandlers
{
    public class PDFExtractionHandler : IEventHandler
    {
        private readonly ILogger _logger;
        private readonly IBlobService _blobService;

        public PDFExtractionHandler(ILogger logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
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

                var contextOptions = new DbContextOptionsBuilder<DataContext>()
                    .UseSqlServer(Environment.GetEnvironmentVariable("SQLConnectionString"), options=>options.EnableRetryOnFailure())
                    .Options;

                using var _context = new DataContext(contextOptions);
  
                IDownloadFile downLoadFile = new DownloadFileHandler(new HttpClient(), _logger);
                IPDFExtraction pdfExtractor = new PDFExtractionPDFText(_logger);

                if (_context.InstitutionReportCriterieaDatabasemodel.Any(x => x.ReportId == pdfExtractionModel.id))
                {
					_logger.Information($"Report {pdfExtractionModel.id} already Exist");
					return true;

				}

				var filePath = Path.GetTempPath();
                _logger.Debug("File path: {filepath}", filePath);
                var filename = pdfExtractionModel.id.ToString() + "." + pdfExtractionModel.documentExtention;
                var fullPath = string.Join("", filePath, "/", filename);
                var file = await downLoadFile.DownloadFile(fullPath, filename, pdfExtractionModel.downloadUrl);

                if (file != null )
                {
                    await ExtractCathegoriesFromPDFAndPersistInDB(pdfExtractionModel, _context, pdfExtractor, fullPath, _logger);
                    await UploadToBLobAndDeleteLocalFile(filename, fullPath, file);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error message: {ex.Message}\nInner Exception: {ex.InnerException}");
                return false;

            }
        }

        private static async Task ExtractCathegoriesFromPDFAndPersistInDB(TilsynsRapportToExtraxtModel pdfExtractionModel, DataContext _context, IPDFExtraction pdfExtractor, string fullPath, ILogger logger)
        {
			logger.Information("Extracting File Nontent");
            Dictionary<string, string> test = null;
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                test = pdfExtractor.Extract_Categories_From_Stream(fileStream);
            }

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

            await _context.SaveChangesAsync();
        }

        private async Task UploadToBLobAndDeleteLocalFile(string filename, string fullPath, string content)
        {
            try
            {
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    await _blobService.UploadBlob(filename, fileStream, Environment.GetEnvironmentVariable("FileStorageContainer"), content);
                }
            }
			catch (RequestFailedException ex) {
                throw;
            }
            finally
            {
				File.Delete(fullPath);

			}
			

		}
    }
}

