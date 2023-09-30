using ModelsLib.DatabaseModels;
using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IInstitutionRepository
    {
        public Guid AddInstitution(InstitutionFrontPageModel institution);
        public Guid UpdateInstitution(InstitutionFrontPageModel institution);
        public InstitutionFrontPageModel GetInstitutionById(Guid institutionId);

        public void Test();

    }
}
