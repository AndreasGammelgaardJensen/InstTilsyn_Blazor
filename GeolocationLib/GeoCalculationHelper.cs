using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationLib
{
	public class GeoCalculationHelper
	{
		public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
		{
			double R = (unit == 'K') ? 6371.0 : 3958.8; // Radius of the Earth in kilometers or miles
			double dLat = ToRadians(lat2 - lat1);
			double dLon = ToRadians(lon2 - lon1);

			double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
								Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
								Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double distance = R * c;

			return Math.Round(distance, 2); ;
		}

		public static double ToRadians(double degrees)
		{
			return degrees * (Math.PI / 180);
		}
	}
}
