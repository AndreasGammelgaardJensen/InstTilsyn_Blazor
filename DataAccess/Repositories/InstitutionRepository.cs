using DataAccess.Database;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsLib.DatabaseModels;
using ModelsLib.Models;
using Serilog;



namespace DataAccess.Repositories
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly DataContext _dbContext;
        private readonly ILogger _logger;

        public InstitutionRepository(DataContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Guid AddInstitution(InstitutionFrontPageModel institution)
        {
            //TODO: Also validate by address Update hash key to be unique
            if ( NameExist(institution.Name)) return Guid.Empty;
            _logger.Information("Adding {institution} to database", institution.Name);
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
            institutionDbModel.TypeEnum = institution.TypeEnum;
            institutionDbModel.CreatedAt = DateTime.Now;
            institutionDbModel.LastChangedAt = DateTime.Now;
            
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

            //_dbContext.InstKoordinatesDatabasemodels.Add(institutionDbModel.Koordinates);
            //_dbContext.InstitutionTilsynsRapportDatabasemodels.AddRange(institutionDbModel.InstitutionTilsynsRapports);
            //_dbContext.PladserDatabaseModel.Add(institutionDbModel.pladser);
            //_dbContext.PladserDatabaseModel.Add(institutionDbModel.pladser);
            //_dbContext.AddressDatabaseModel.Add(institutionDbModel.address);
            _dbContext.InstitutionFrontPageModel.Add(institutionDbModel);

            _dbContext.SaveChanges();
            _logger.Information("{institution} Added to database", institution.Name);

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
            _logger.Information("Updating {institution} in database", institution.Name);
            var institutionDbModel = _dbContext.InstitutionFrontPageModel.Include(x=> x.pladser).Include(x=>x.InstitutionTilsynsRapports).Include(x=>x.Koordinates).Include(x=>x.address).SingleOrDefault(x => x.Name == institution.Name);
            
            institutionDbModel.profile = institution.profile;
            institutionDbModel.homePage = institution.HomePage;

            //Update pladser
            var pladser = institutionDbModel.pladser;
            if (pladser.VuggestuePladser != institution.pladser.VuggestuePladser)
            {
                _logger.Information("Vuggestuepladser updatet from {old} to {new}", pladser.VuggestuePladser, institution.pladser.VuggestuePladser);
            }else if(pladser.BoernehavePladser != institution.pladser.BoernehavePladser)
            {
                _logger.Information("BoernehavePladser updatet from {old} to {new}", pladser.BoernehavePladser, institution.pladser.BoernehavePladser);
            }
            else if (pladser.DagplejePladser != institution.pladser.DagplejePladser)
            {
                _logger.Information("DagplejePladser updatet from {old} to {new}", pladser.DagplejePladser, institution.pladser.DagplejePladser);
            }

            pladser.LastChangedAt = DateTime.Now;
            pladser.VuggestuePladser = institution.pladser.VuggestuePladser;
            pladser.BoernehavePladser = institution.pladser.BoernehavePladser;
            pladser.DagplejePladser = institution.pladser.DagplejePladser;

            

            //Update Tilsynsrapporter
            var tilsynsrapporter = institutionDbModel.InstitutionTilsynsRapports;
            var dbResHash = tilsynsrapporter.Select(selector => selector.hash).ToList();
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
            _logger.Information("{institution} Updated in database", institution.Name);

            return institutionDbModel.Id;
        }

        private bool NameExist(string name)
        {
           return _dbContext.InstitutionFrontPageModel.Any(x => x.Name == name);
        }
    }
}
