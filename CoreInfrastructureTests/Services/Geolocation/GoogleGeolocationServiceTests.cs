using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreInfrastructure.Services.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLib.Models;

namespace CoreInfrastructure.Services.Geolocation.Tests
{
	[TestClass()]
	public class GoogleGeolocationServiceTests
	{
		[TestMethod()]
		public void GetKoordinatesFromAddressesAsyncTest()
		{
			IGoogleGeolocationService geolocationService = new GoogleGeolocationService(new HttpClient(), "AIzaSyAzAuTdxDufGTBujm4Y-dNBXVs9aLFw23M", "https://maps.googleapis.com/maps/api/geocode/json");

			var addresses = new List<Address>
			{
				new Address
				{
					Vej ="Kløverbakkevej",
					Number = "13",
					Zip_code = 8300,
					City = "Odder"
				},
				new Address {
					Vej ="Klostervej",
					Number = "20",
					Zip_code = 8680,
					City = "Ry"
				}
			};


			var response = geolocationService.GetKoordinatesFromAddressesAsync(addresses).Result;

			var test = true;
		}
	}
}