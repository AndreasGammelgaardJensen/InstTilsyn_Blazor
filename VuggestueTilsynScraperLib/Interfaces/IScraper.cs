using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VuggestueTilsynScraperLib.Interfaces
{
    public interface IScraper
    {
        public void BeginScraping(Action<Guid, List<InstitutionTilsynsRapport>> onScrapingResult);
        public void InitializeScraping();
    }
}
