using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFExtractionLib.Handlers
{
    public interface IDownloadFile
    {
        public Task<string> DownloadFile(string filepath, string filename, string downloadUrl);
    }
}
