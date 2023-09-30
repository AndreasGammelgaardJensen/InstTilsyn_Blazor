using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.DatabaseModels
{

    public class InstitutionTilsynsRapportDatabasemodel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public string Name { get; set; }
        public string documentType { get; set; }
        public string fileUrl { get; set; }
        public DateTime copyDate { get; set; }
        public string hash { get; set; }
    }
}
