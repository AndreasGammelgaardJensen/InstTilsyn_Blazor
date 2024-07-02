using CoreInfrastructure.Services.Geolocation;
using Microsoft.AspNetCore.Mvc;
using ModelsLib.Models;
using ModelsLib.ResponseModels;

namespace TilsynsRapportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoordinateController : ControllerBase
    {
		private readonly IGoogleGeolocationService _googleGeplocationService;

		public CoordinateController(IGoogleGeolocationService googleGeplocationService)
		{
			_googleGeplocationService = googleGeplocationService;
		}

		[HttpPost]
		public async Task<InstitutionSettingsFilterModel?> Post([FromBody] Address value)
		{
			var coordinateList = await _googleGeplocationService.GetKoordinatesFromAddressesAsync(new List<Address> { value });

			var koordinate = new InstKoordinates
			{
				lat = coordinateList.Results.First().Geometry.Location.Lat,
                lng = coordinateList.Results.First().Geometry.Location.Lng
            };

			

			return new InstitutionSettingsFilterModel { Address = value, Koordinates = koordinate };


        }
	}
}
