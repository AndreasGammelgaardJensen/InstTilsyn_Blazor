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

            extractor.Extract_Categories("C:/Users/andreas.jensen/source/repos/TilsynVuggestueBlazer/VuggestueTilsyn/VuggestueTilsynScraperLibTest/Resources/Pdf/boernehavenhaabet2015.pdf");

        }
    }
}
