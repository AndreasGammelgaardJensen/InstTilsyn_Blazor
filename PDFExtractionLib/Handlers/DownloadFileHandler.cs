
using Serilog;
using System.Net.Http;


namespace PDFExtractionLib.Handlers
{
    public class DownloadFileHandler : IDownloadFile
    {

        private readonly ILogger _logger;

        public DownloadFileHandler(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private HttpClient _httpClient { get; set; }

        public async Task<string> DownloadFile(string filepath, string filename, string downloadUrl)
        {
                      
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(downloadUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        using (var contentStream = await response.Content.ReadAsStreamAsync())


                        using (var fileStream = System.IO.File.Create(string.Join("", filepath, "/", filename)))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }

                        _logger.Information("PDF file downloaded successfully!");
                    }
                    else
                    {
                        _logger.Warning($"Failed to download PDF. Status code: {response.StatusCode} : filename: {filename}");
                    }
                    return response.Content.Headers.ContentType.ToString();
                
            }
                catch (Exception ex)
                {
                    _logger.Error($"An error occurred: {ex.Message}");
                    return null;
                }finally { _httpClient.Dispose(); }
                

        }

    }
}
