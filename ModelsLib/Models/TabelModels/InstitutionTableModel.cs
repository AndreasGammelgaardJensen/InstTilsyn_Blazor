using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models.TabelModels
{
    public class InstitutionTableModel
    {
        public InstitutionFrontPageModel InstitutionFrontPageModel { get; set; }
        public List<CriteriaModel> CriteriaModel { get; set; }
    }
}
