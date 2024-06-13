using ModelsLib.Models;
using ModelsLib.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationLib.Google
{
	public class LocationMapper
	{
		public static List<InstitutionFrontPageModel> MapKoordinatesToInstitutions(List<Result> koordinateResult, List<InstitutionFrontPageModel> institutionModels)
		{
			koordinateResult.ForEach(result =>
			{

				try
				{

					var streetName = result.AddressComponents.Where(x => x.Types.Contains("route"));
					var streetNumber = result.AddressComponents.Where(x => x.Types.Contains("street_number"));

					switch (streetName.Count())
					{
						case 0:
							{
								break;
							}
						case 1:
							{
								var address = streetName.FirstOrDefault()?.LongName.ToLower();

								var number = streetNumber.FirstOrDefault()?.LongName.ToLower();

								var inst = institutionModels.SingleOrDefault(x =>string.Equals(x.address.Number??"", number, StringComparison.OrdinalIgnoreCase)  && string.Equals(x.address.Vej ?? "", address, StringComparison.OrdinalIgnoreCase));
								if (inst is not null)
								{
									var latKoordinate = result.Geometry.Location.Lat;
									var lngKoordinate = result.Geometry.Location.Lng;

									inst.Koordinates.lat = latKoordinate;
									inst.Koordinates.lng = lngKoordinate;
								}

								break;
							}

						default: { break; }
					}

				}catch (Exception ex)
				{

					Console.WriteLine("Skipping ResultSet");
				}
				});
			return institutionModels;
		}
	}
}
