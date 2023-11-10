using DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelsLib.Models;
using ModelsLib.Models.TabelModels;
using System.Collections.Generic;
using System.Diagnostics;
using TilsynsRapportApi.Database;

namespace TilsynsRapportApi.Repositories
{
    public class InstitutionTabelReporisory : IInstitutionTableRepository
    {
        private readonly ReportDBContext _reportDBContext;
        private readonly BaseContext _institutionDBContext;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;


        public InstitutionTabelReporisory(BaseContext institutionDBContext, ReportDBContext reportDBContext, IMemoryCache memoryCache)
        {
            _institutionDBContext = institutionDBContext;
            _reportDBContext = reportDBContext;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<InstitutionTableModel>> GetInstitutionTableModels()
        {
            //List<InstitutionTableModel> institutionTalebLis = _memoryCache.Get<List<InstitutionTableModel>>("INSTITUTION_TABEL_MODELS");

            //if (institutionTalebLis is not null)
            //    return institutionTalebLis;

            List<InstitutionTableModel>  institutionTalebLis = new();
                Stopwatch stopWatch = new Stopwatch();
            int counter = 0;
            try
            {
                stopWatch.Start();

                var testCri = _reportDBContext.InstitutionReportCriterieaDatabasemodel.Include(rr => rr.Categories).ToListAsync();
                var testInst = _institutionDBContext.InstitutionFrontPageModel
                    .Include(insfp => insfp.pladser)
                    .Include(insfp => insfp.InstitutionTilsynsRapports)
                    .Include(insfp => insfp.Koordinates)
                    .Include(insfp => insfp.address)
                    .ToListAsync();

                await Task.WhenAll(testCri, testInst);

                stopWatch.Stop();

                testInst.Result.ForEach(inst =>
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

                    if (inst.InstitutionTilsynsRapports.Any())
                    {
                        var rap = inst.InstitutionTilsynsRapports.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                        instPageModel.InstitutionTilsynsRapports = new List<InstitutionTilsynsRapport>
                        {
                            new InstitutionTilsynsRapport { fileUrl = rap.fileUrl }
                        };
                    }


                    var instTabelModel = new InstitutionTableModel();
                    instTabelModel.InstitutionFrontPageModel = instPageModel;
                    instTabelModel.CriteriaModel = new List<CriteriaModel>();

                    //Create Criterie
                    var criteriea = testCri.Result.SingleOrDefault(cr => cr.ReportId == inst.InstitutionTilsynsRapports?.OrderByDescending(x => x.CreatedAt).FirstOrDefault()?.Id)?.Categories;

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

                    institutionTalebLis.Add(instTabelModel);
                });

                _memoryCache.Set<List<InstitutionTableModel>>("INSTITUTION_TABEL_MODELS", institutionTalebLis, TimeSpan.FromMinutes(1));

                return institutionTalebLis;
            }catch(Exception e)
            {
                
                Console.WriteLine(e.ToString());
                return null;
            }
            
            
        }
    }
}
