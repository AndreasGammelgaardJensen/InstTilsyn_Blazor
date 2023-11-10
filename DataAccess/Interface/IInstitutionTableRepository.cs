using ModelsLib.Models.TabelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IInstitutionTableRepository
    {
        public Task<IEnumerable<InstitutionTableModel>> GetInstitutionTableModels();
    }
}
