using ModelsLib.Models;
using ModelsLib.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Services.Geolocation
{
	public interface IGoogleGeolocationService
	{
		public Task<GoogleGeolocationResponseModel?> GetKoordinatesFromAddressesAsync(List<Address> addresses);
	}
}
