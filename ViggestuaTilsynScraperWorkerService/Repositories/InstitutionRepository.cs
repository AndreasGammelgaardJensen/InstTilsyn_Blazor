
using DataAccess.Interfaces;
using ViggestuaTilsynScraperWorkerService.Database;


namespace ViggestuaTilsynScraperWorkerService.Repositories
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly DataContext _dbContext;

        public InstitutionRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid AddInstitution(InstitutionFrontPageModel institution)
        {
            //TODO: Also validate by address Update hash key to be unique
            if ( NameExist(institution.Name)) return Guid.Empty;


            var institutionDbModel = new InstitutionFrontPageModelDatabasemodel();

            institutionDbModel.Id = institution.Id;
            institutionDbModel.homePage = institution.HomePage;
            institutionDbModel.Name = institution.Name;

            if(institution.pladser != null)
            {
                institutionDbModel.pladser = new PladserDatabasemodel
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    LastChangedAt = DateTime.Now,
                    VuggestuePladser = institution.pladser.VuggestuePladser,
                    BoernehavePladser = institution.pladser.BoernehavePladser,
                    DagplejePladser = institution.pladser.DagplejePladser,
                };
            }

            
            institutionDbModel.profile = institution.profile;

            if(institution.address != null)
                institutionDbModel.address = new AddressDatabasemodel { 
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    LastChangedAt = DateTime.Now,
                    Vej = institution.address.Vej,
                    City = institution.address.City,
                    Zip_code = institution.address.Zip_code,
                };
            institutionDbModel.homePage = institution.HomePage; 
            institutionDbModel.TypeEnum = institution.TypeEnum;
            
            if(institution.InstitutionTilsynsRapports.Any())
            {
                institutionDbModel.InstitutionTilsynsRapports = new List<InstitutionTilsynsRapportDatabasemodel>();
                institution.InstitutionTilsynsRapports.ForEach(x =>
                {
                    var newTilsynsrapportInstitutionModel = new InstitutionTilsynsRapportDatabasemodel
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        LastChangedAt = DateTime.Now,
                        copyDate = x.copyDate,
                        Name = x.Name,
                        fileUrl = x.fileUrl,
                        documentType = x.documentType,
                        hash = x.hash,
                    };

                    institutionDbModel.InstitutionTilsynsRapports.Add(newTilsynsrapportInstitutionModel);

                });
            }
            
            institutionDbModel.Koordinates = new InstKoordinatesDatabasemodel
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                LastChangedAt = DateTime.Now,
                lat = institution.Koordinates.lat,
                lng = institution.Koordinates.lng,
            };


            _dbContext.InstitutionFrontPageModel.Add(institutionDbModel);

            _dbContext.SaveChanges();

            return institutionDbModel.Id;
        }

        public InstitutionFrontPageModel GetInstitutionById(Guid institutionId)
        {
            var institutionDbModel = _dbContext.InstitutionFrontPageModel.Include(x => x.pladser).Include(x => x.InstitutionTilsynsRapports).Include(x => x.Koordinates).Include(x => x.address).SingleOrDefault(x => x.Id == institutionId);

            var institutionFPM = new InstitutionFrontPageModel();

            institutionFPM.Id = institutionDbModel.Id;
            institutionFPM.profile = institutionDbModel.profile;
            institutionFPM.InstitutionTilsynsRapports = new List<InstitutionTilsynsRapport>();

            institutionDbModel.InstitutionTilsynsRapports.ForEach(repDbModel =>
            {
                institutionFPM.InstitutionTilsynsRapports.Add(new InstitutionTilsynsRapport
                {
                    Id = repDbModel.Id,
                    fileUrl = repDbModel.fileUrl,
                    hash = repDbModel.hash,
                    documentType = repDbModel.documentType,
                    copyDate = repDbModel.copyDate,
                    Name = repDbModel.Name
                });
            });

            return institutionFPM;

        }

        public void Test()
        {
            throw new NotImplementedException();
        }

        public Guid UpdateInstitution(InstitutionFrontPageModel institution)
        {
            
            var institutionDbModel = _dbContext.InstitutionFrontPageModel.Include(x=> x.pladser).Include(x=>x.InstitutionTilsynsRapports).Include(x=>x.Koordinates).Include(x=>x.address).SingleOrDefault(x => x.Name == institution.Name);
            
            institutionDbModel.profile = institution.profile;
            institutionDbModel.homePage = institution.HomePage;

            //Update pladser
            var pladser = institutionDbModel.pladser;
            pladser.LastChangedAt = DateTime.Now;
            pladser.VuggestuePladser = institution.pladser.VuggestuePladser;
            pladser.BoernehavePladser = institution.pladser.BoernehavePladser;
            pladser.DagplejePladser = institution.pladser.DagplejePladser;

            //Update Tilsynsrapporter
            var tilsynsrapporter = institutionDbModel.InstitutionTilsynsRapports;
            var dbResHash = tilsynsrapporter.Select(selector => selector.hash).ToList();
            //TODO: Sikre at det er det samme Id der bliver overført også selv om at rapport er gemt i forvejen.
            var restTilsynsRapport = institution.InstitutionTilsynsRapports.Where(instTRep => !dbResHash.Contains(instTRep.hash)).ToList();
            institution.InstitutionTilsynsRapports = restTilsynsRapport;

            institution.InstitutionTilsynsRapports.ForEach(x =>
            {
                if(!tilsynsrapporter.Any(rDb=> rDb.hash == x.hash))
                {
                    tilsynsrapporter.Add(new InstitutionTilsynsRapportDatabasemodel
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        LastChangedAt = DateTime.Now,
                        copyDate = x.copyDate,
                        Name = x.Name,
                        fileUrl = x.fileUrl,
                        documentType = x.documentType,
                        hash = x.hash
                    });
                }
            });


            //Update address
            var address = institutionDbModel.address;
            address.LastChangedAt = DateTime.Now;
            address.Vej = institution.address.Vej;
            address.City = institution.address.City;
            address.Zip_code = institution.address.Zip_code;


            //Update coordianetes
            var koordinates = institutionDbModel.Koordinates;
            koordinates.LastChangedAt = DateTime.Now;
            koordinates.lat = institution.Koordinates.lat;
            koordinates.lng = institution.Koordinates.lng;

            _dbContext.Update(institutionDbModel);
            _dbContext.SaveChanges();

            return institutionDbModel.Id;
        }

        private bool NameExist(string name)
        {
           return _dbContext.InstitutionFrontPageModel.Any(x => x.Name == name);
        }
    }
}
