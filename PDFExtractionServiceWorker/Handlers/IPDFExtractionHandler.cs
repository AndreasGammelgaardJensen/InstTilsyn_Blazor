using ModelsLib.Models.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFExtractionServiceWorker.Handlers
{
    public interface IPDFExtractionHandler
    {
        public Task<bool> HandelPdf(TilsynsRapportToExtraxtModel pdfExtractionModel);
    }
}
