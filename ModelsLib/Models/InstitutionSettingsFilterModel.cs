namespace ModelsLib.Models
{
	public class InstitutionSettingsFilterModel
	{
		public Address Address { get; set; }
		public InstKoordinates Koordinates { get; set; }
		public IEnumerable<InstitutionTypeEnum> InstitutionTypes { get; set; }
		public int FilterCount { get; set; }
	}
}
