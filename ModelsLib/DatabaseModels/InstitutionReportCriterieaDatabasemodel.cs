using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.DatabaseModels
{
    public class InstitutionReportCriterieaDatabasemodel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public Guid ReportId { get; set; }
        public Guid InstitutionId { get; set; }
        public string fileUrl { get; set; }
        public List<CategoriClass> Categories { get; set; }

        //public CategoriClass Samspil_og_relationer_mellem_børn_Id { get; set; }
        //public CategoriClass Børnefællesskaber_og_leg_Id { get; set; }
        //public CategoriClass Sprog_og_bevægelse_Id { get; set; }
        //public CategoriClass Forældresamarbejde_Id { get; set; }
        //public CategoriClass Sammenhæng_i_overgange_Id { get; set; }
        //public CategoriClass Evalueringskultur_Id { get; set; }
    }

    public class CategoriClass
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public string CategoriText { get; set; }
        public string Indsats { get; set; }


    }
}
