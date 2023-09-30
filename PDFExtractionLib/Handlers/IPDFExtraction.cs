using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFExtractionLib.Handlers
{
    public interface IPDFExtraction
    {
        public Dictionary<string, string> Extract_Categories(string filePath);
        public void Test();
    }
}
