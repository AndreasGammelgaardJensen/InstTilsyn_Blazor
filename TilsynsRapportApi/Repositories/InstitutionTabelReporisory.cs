using DataAccess.Database;
using DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelsLib.Models;
using ModelsLib.Models.TabelModels;
using System.Collections.Generic;
using System.Diagnostics;

namespace TilsynsRapportApi.Repositories
{
    public class InstitutionTabelReporisory : IInstitutionTableRepository
    {
        private readonly DataContext _reportDBContext;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;


        public InstitutionTabelReporisory(DataContext reportDBContext, IMemoryCache memoryCache)
        {
            _reportDBContext = reportDBContext;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<InstitutionTableModel>> GetInstitutionTableModels()
        {
            List<InstitutionTableModel> institutionTalebLis = _memoryCache.Get<List<InstitutionTableModel>>("INSTITUTION_TABEL_MODELS");

            if (institutionTalebLis is not null)
                return institutionTalebLis;

            institutionTalebLis = new();
				Stopwatch stopWatch = new Stopwatch();
            int counter = 0;
            try
            {
                stopWatch.Start();

                var testCri = await _reportDBContext.InstitutionReportCriterieaDatabasemodel.Include(rr => rr.Categories).ToListAsync();
                var testInst = await _reportDBContext.InstitutionFrontPageModel
                    .Include(insfp => insfp.pladser)
                    .Include(insfp => insfp.InstitutionTilsynsRapports)
                    .Include(insfp => insfp.Koordinates)
                    .Include(insfp => insfp.address)
                    .Include(insfp => insfp.ContactDatabasemodel)
					.ToListAsync();


                stopWatch.Stop();

                testInst.ForEach(async inst =>
                {
                    counter++;
                    var instPageModel = new InstitutionFrontPageModel();
                    instPageModel.Id = inst.Id;
                    instPageModel.Name = inst.Name;
                    instPageModel.HomePage = inst.homePage;
                    instPageModel.TypeEnum = inst.TypeEnum;

                    if (inst.pladser != null)
                    {
                        instPageModel.pladser = new Pladser
                        {
                            VuggestuePladser = inst.pladser.VuggestuePladser,
                            BoernehavePladser = inst.pladser.BoernehavePladser,
                            DagplejePladser = inst.pladser.DagplejePladser,
                        };
                    }

                    if (inst.address != null)
                    {
                        instPageModel.address = new Address
                        {
                            City = inst.address.City,
                            Number = inst.address.Number,
                            Zip_code = inst.address.Zip_code,
                            Id = inst.address.Id,
                            Vej = inst.address.Vej,
                        };
                    }

                    if (inst.Koordinates != null)
                    {
                        instPageModel.Koordinates = new InstKoordinates
                        {
                            Id = inst.Koordinates.Id,
                            lat = inst.Koordinates.lat,
                            lng = inst.Koordinates.lng,
                        };
                    }

					if (inst.ContactDatabasemodel != null)
					{
						instPageModel.Contact = new Contact
						{
							Id = inst.ContactDatabasemodel.Id,
							HomePage = inst.ContactDatabasemodel.HomePage,
							Email = inst.ContactDatabasemodel.Email,
							Phone = inst.ContactDatabasemodel.Phone
						};
					}

					var repports = new List<ReportTabelModel>();

                    if (inst.InstitutionTilsynsRapports.Any())
                    {
                        var rap = inst.InstitutionTilsynsRapports.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        instPageModel.InstitutionTilsynsRapports = new List<InstitutionTilsynsRapport>
                        {
                            new InstitutionTilsynsRapport { fileUrl = rap.fileUrl }
                        };
                        var reportIds = inst.InstitutionTilsynsRapports.Select(x=>x.Id).ToList();

						var tt = testCri.Where(x => reportIds.Contains(x.ReportId)).ToList();                    
                        
                        repports = inst.InstitutionTilsynsRapports.OrderByDescending(x => x.CreatedAt).Select(x=> new ReportTabelModel
                        {
                            ReportId = x.Id,
                            FileUrl = x.fileUrl,
                            Criteria = tt.FirstOrDefault(t => t.ReportId == x.Id)?.Categories?.Select(cr => new CriteriaModel { CategoriText = cr.CategoriText, Indsats = cr.Indsats }).ToList()
                        }).ToList();

					}


                    var instTabelModel = new InstitutionTableModel();
                    instTabelModel.InstitutionFrontPageModel = instPageModel;
                    instTabelModel.CriteriaModel = new List<CriteriaModel>();

                    //Create Criterie
                    var criteriea = testCri.SingleOrDefault(cr => cr.ReportId == inst.InstitutionTilsynsRapports?.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.Id)?.Categories;

                    if (criteriea != null && criteriea.Any())
                    {
                        criteriea.OrderBy(x => x.CategoriText).ToList().ForEach(cr =>
                        {
                            if (cr != null)
                                instTabelModel.CriteriaModel.Add(new CriteriaModel
                                {
                                    Indsats = cr.Indsats,
                                    CategoriText = cr.CategoriText,
                                });

                        });
                    }

                    instTabelModel.ShowAdditionalInfo = false;
                    instTabelModel.ReportTabelModel = repports;

					institutionTalebLis.Add(instTabelModel);
                });

                _memoryCache.Set<List<InstitutionTableModel>>("INSTITUTION_TABEL_MODELS", institutionTalebLis, TimeSpan.FromMinutes(10));

                return institutionTalebLis;
            }catch(Exception e)
            {
                
                Console.WriteLine(e.ToString());
                return null;
            }
            
            
        }
    }
}
