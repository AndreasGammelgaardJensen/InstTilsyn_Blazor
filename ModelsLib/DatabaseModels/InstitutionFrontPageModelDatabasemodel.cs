using ModelsLib.Models;

namespace ModelsLib.DatabaseModels
{
    public class InstitutionFrontPageModelDatabasemodel
    {
    
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChangedAt { get; set; }
        public InstitutionTypeEnum TypeEnum { get; set; }
        public string Name { get; set; }
        public AddressDatabasemodel? address { get; set; }
        public PladserDatabasemodel? pladser { get; set; }
        public string? homePage { get; set; }
        public string? profile { get;set; }
        public List<InstitutionTilsynsRapportDatabasemodel> InstitutionTilsynsRapports { get; set; }
        public InstKoordinatesDatabasemodel? Koordinates { get; set; }
    }

}
