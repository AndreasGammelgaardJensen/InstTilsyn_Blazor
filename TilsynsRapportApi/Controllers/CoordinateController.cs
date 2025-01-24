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
		private readonly ILogger<CoordinateController> _logger;


		public CoordinateController(IGoogleGeolocationService googleGeplocationService, ILogger<CoordinateController> logger)
		{
			_googleGeplocationService = googleGeplocationService;
			_logger = logger;
		}

		[HttpPost]
		public async Task<InstitutionSettingsFilterModel?> Post([FromBody] Address value)
		{
			_logger.LogInformation("Post Coordinates");
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
