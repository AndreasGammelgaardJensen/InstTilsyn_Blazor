using Microsoft.AspNetCore.Http;

namespace PDFExtractionLib.Handlers
{
    public interface IDownloadFile
    {
        public Task<string> DownloadFile(string filepath, string filename, string downloadUrl);
    }
}
