using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VuggestueTilsynScraperLib.Validation
{
    public class Validation
    {
        public static bool ValidateInstitutionModel(InstitutionFrontPageModel institutionModel)
        {

            var reportsList = new List<InstitutionTilsynsRapport>();

            institutionModel.InstitutionTilsynsRapports.ForEach(report =>
            {
                //Do validate of report name and other stuff
                if (report.Name != null)
                    reportsList.Add(report);
            });

            institutionModel.InstitutionTilsynsRapports = reportsList;

            if (institutionModel.Name == null) return false;

            return true;
        } 
    }
}
