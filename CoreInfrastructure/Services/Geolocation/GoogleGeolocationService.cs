using GeolocationLib.Google;
using ModelsLib.Models;
using ModelsLib.Models.TabelModels;
using ModelsLib.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CoreInfrastructure.Services.Geolocation
{
	public class GoogleGeolocationService : IGoogleGeolocationService
	{
		private HttpClient _httpClient;
		private readonly string _API_KEY;
		private readonly string _baseUrl;

		public GoogleGeolocationService(HttpClient httpClient, string API_KEY, string baseUrl)
		{
			_httpClient = httpClient;
			_API_KEY = API_KEY;
			_baseUrl = baseUrl;
			_httpClient.BaseAddress = new Uri(_baseUrl);
		}


		public async Task<GoogleGeolocationResponseModel?> GetKoordinatesFromAddressesAsync(List<Address> addresses)
		{
			var param = LocationHelper.GetFormattedAddresses(addresses);
			var uri = $"?address={param}&key={_API_KEY}";

			var res = await _httpClient.GetAsync(uri);
			var test = await res.Content.ReadAsStringAsync();
			GoogleGeolocationResponseModel response = JsonConvert.DeserializeObject<GoogleGeolocationResponseModel>(test);

			return response;
		}

	}
}
