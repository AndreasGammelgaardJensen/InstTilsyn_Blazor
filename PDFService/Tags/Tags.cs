using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFService.Tags
{
    public class Tags
    {
        //Varsion 1
        public static readonly string Samspil_og_relationer = "Samspil og relationer mellem børn";
        public static readonly string Boernefællesskaber_og_leg = "Børnefællesskaber og leg";
        public static readonly string Sprog_og_bevaegelse = "Sprog og bevægelse";
        public static readonly string Foraeldresamarbejde = "Forældresamarbejde";
        public static readonly string Sammenhaeng_i_overgange = "Sammenhæng i overgange";
        public static readonly string Evalueringkultur = "Evalueringskultur";

        //Version 2
        public static readonly string Samspil_og_relationer_indsats = "Sociale relationer – positiv voksenkontakt hver dag";
        public static readonly string Boernefællesskaber_og_leg_indsats = "Inklusion og fællesskab – børne- og ungefællesskaber til alle";
        public static readonly string Sprog_og_bevaegelse_indsats = "Sprogindsatsen – muligheder gennem sprog";
        public static readonly string Foraeldresamarbejde_indsats = "Forældresamarbejde - forældrepartnerskab";
        public static readonly string Sammenhaeng_i_overgange_indsats = "Sammenhæng - også i overgange";
        public static readonly string Evalueringkultur_indsats = "Krav om refleksion og metodisk systematik i den pædagogiske praksis";

        public static readonly string Section_Vurdering = "Det vurderes på baggrund af observationer og faglig dialog, at institutionen skal";
        public static readonly string Section_Vurdering_v2 = "Det vurderes på baggrund af observationer og faglig dialog, at institutionen skal";
        public static readonly string Section_Indsats = "Indsats";


        public static readonly List<string> CategoriList = new List<string> { Samspil_og_relationer,
            Boernefællesskaber_og_leg,
            Sprog_og_bevaegelse,
            Foraeldresamarbejde,
            Sammenhaeng_i_overgange,
            Evalueringkultur,
            Samspil_og_relationer_indsats,
            Boernefællesskaber_og_leg_indsats,
            Sprog_og_bevaegelse_indsats,
            Foraeldresamarbejde_indsats,
            Sammenhaeng_i_overgange_indsats, 
            Evalueringkultur_indsats};





        public static Categories Evaluate_text_contains_categori(string text)
        {
            var textCopy = text.Replace("\n", "").Trim();

            foreach (var item in CategoriList)
            {
                if (textCopy.Contains(item))
                    return TagConverterToEnum(item.Trim());
            }
            return Categories.None;
        }



        public static string TagConverter(Categories x)
        {
            return x switch
            {
                Categories.Samspil_og_relationer => Samspil_og_relationer,
                Categories.Boernefællesskaber_og_leg => Boernefællesskaber_og_leg,
                Categories.Sprog_og_bevaegelse => Sprog_og_bevaegelse,
                Categories.Foraeldresamarbejde => Foraeldresamarbejde,
                Categories.Sammenhaeng_i_overgange => Sammenhaeng_i_overgange,
                Categories.Evalueringkultur => Evalueringkultur,

                Categories.Samspil_og_relationer_indsats => Samspil_og_relationer_indsats,
                Categories.Boernefællesskaber_og_leg_indsats => Boernefællesskaber_og_leg_indsats,
                Categories.Sprog_og_bevaegelse_indsats => Sprog_og_bevaegelse_indsats,
                Categories.Foraeldresamarbejde_indsats => Foraeldresamarbejde_indsats,
                Categories.Sammenhaeng_i_overgange_indsats => Sammenhaeng_i_overgange_indsats,
                Categories.Evalueringkultur_indsats => Evalueringkultur_indsats,
                // Add more cases for other enum values if needed
                _ => throw new ArgumentException("Unknown category categori", nameof(x))
            };
        }

        public static Categories TagConverterToEnum(string x)
        {
            return x switch
            {
                "Samspil og relationer mellem børn" => Categories.Samspil_og_relationer,
                "Børnefællesskaber og leg" => Categories.Boernefællesskaber_og_leg,
                "Sprog og bevægelse" => Categories.Sprog_og_bevaegelse,
                "Forældresamarbejde" => Categories.Foraeldresamarbejde,
                "Sammenhæng i overgange" => Categories.Sammenhaeng_i_overgange,
                "Evalueringskultur" => Categories.Evalueringkultur,

                "Sociale relationer – positiv voksenkontakt hver dag" => Categories.Samspil_og_relationer_indsats,
                "Inklusion og fællesskab – børne- og ungefællesskaber til alle" => Categories.Boernefællesskaber_og_leg_indsats,
                "Sprogindsatsen – muligheder gennem sprog" => Categories.Sprog_og_bevaegelse_indsats,
                "Forældresamarbejde - forældrepartnerskab" => Categories.Foraeldresamarbejde_indsats,
                "Sammenhæng - også i overgange" => Categories.Sammenhaeng_i_overgange_indsats,
                "Krav om refleksion og metodisk systematik i den pædagogiske praksis" => Categories.Evalueringkultur_indsats,
                // Add more cases for other enum values if needed
                _ => throw new ArgumentException("Unknown category categori", nameof(x))
            };
        }
    }

    
    public enum Categories
    {
        //TOD0: Make this list larger, with all the text types
        Samspil_og_relationer = 0,
        Boernefællesskaber_og_leg = 1,
        Sprog_og_bevaegelse = 2,
        Foraeldresamarbejde = 3,
        Sammenhaeng_i_overgange = 4,
        Evalueringkultur = 5,

        Samspil_og_relationer_indsats = 6,
        Boernefællesskaber_og_leg_indsats = 7,
        Sprog_og_bevaegelse_indsats = 8,
        Foraeldresamarbejde_indsats = 9,
        Sammenhaeng_i_overgange_indsats = 10,
        Evalueringkultur_indsats = 11,
        None = 12,
    }

    public enum ReportType
    {
        Pejlemaerke = 0,
        Vurdering = 1,
    }
}
