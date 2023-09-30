using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models
{

    public class InstitutionTilsynsRapport
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string documentType { get; set; }
        public string fileUrl { get; set; }
        public DateTime copyDate { get; set; }
        public string hash { get; set; }

    }
}
