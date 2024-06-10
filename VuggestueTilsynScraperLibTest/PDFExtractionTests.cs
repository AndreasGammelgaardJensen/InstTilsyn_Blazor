using PDFExtractionLib.Handlers;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VuggestueTilsynScraperLibTest
{
    public class PDFExtractionTests
    {
        [Fact]
        public void Extract_Criteria_Test()
        {
            PDFExtractionPDFText extractor = new PDFExtractionPDFText(null);

            extractor.Extract_Categories("C:/Practice/InstTilsyn_Blazor/PDFExtractionLib/TestFiles/2266.pdf");

        }
    }
}
