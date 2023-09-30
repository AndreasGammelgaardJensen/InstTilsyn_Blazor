using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models
{
    public class InstitutionFrontPageModel
    {

        public Guid Id { get; set; }
        public InstitutionTypeEnum TypeEnum { get; set; }
        public string Name { get; set; }
        public Address address { get; set; }
        public Pladser pladser { get; set; }
        public string HomePage { get; set; }
        public string profile { get; set; }
        public List<InstitutionTilsynsRapport> InstitutionTilsynsRapports { get; set; } = new List<InstitutionTilsynsRapport>();
        public InstKoordinates Koordinates { get; set; }


        public static InstitutionTypeEnum InstitutionTypeTranstator(string translation)
        {

            return translation switch
            {
                "Integreret institution" => InstitutionTypeEnum.Integreret,
                "Dagpleje" => InstitutionTypeEnum.Dagpleje,
                "Vuggestue" => InstitutionTypeEnum.Vuggestue,
                "Børnehave" => InstitutionTypeEnum.Boernehave,
                // Add other cases for other translations here
                _ => throw new ArgumentException("Invalid translation"),
            };
        }


        public string ToString()
        {
            return string.Format("ID: {0}\nName: {1}\nPladser: {2}\nHomePage: {3}\nAddress: {4}\n Type: {5}\n ReportUrl: {6}\n FileType: {7}", Id, Name, string.Format("{0}/{1}/{2}", pladser.VuggestuePladser, pladser.BoernehavePladser, pladser.DagplejePladser), HomePage, address.ToString(), TypeEnum, InstitutionTilsynsRapports.First().fileUrl, InstitutionTilsynsRapports.First().documentType);
        }
    }

    public enum InstitutionTypeEnum
    {
        Integreret = 0,
        Dagpleje = 1,
        Boernehave = 2,
        Vuggestue = 3,

    }   
}
