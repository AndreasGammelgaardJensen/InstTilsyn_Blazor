using ModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeolocationLib.Google
{
	public static class LocationHelper
	{
		// Static method to format and combine a list of addresses
		public static string GetFormattedAddresses(List<Address> addresses)
		{
			List<string> formattedAddresses = new List<string>();

			foreach (var address in addresses)
			{
				formattedAddresses.Add(FormatSingleAddress(address));
			}

			return string.Join("|", formattedAddresses);
		}
		private static string FormatSingleAddress(Address address)
		{
			// Replace white spaces with "+" in Vej
			string formattedVej = address.Vej.Replace(" ", "+");

			return $"{address.Number}+{formattedVej},+{address.Zip_code},+{address.City}";
		}
	}
}
