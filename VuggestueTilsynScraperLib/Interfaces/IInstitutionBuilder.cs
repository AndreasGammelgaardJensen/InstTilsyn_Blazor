using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VuggestueTilsynScraperLib.Interfaces
{
    public interface IInstitutionBuilder
    {
        public InstitutionFrontPageModel BuildBaseInformation();
        public Address BuildAdress();
        public Pladser BuildPladser();
        public InstitutionTilsynsRapport BuildInstitutionTilsynsRapport();

        public InstKoordinates BuildInstitutionCoordinates();
    }
}
