using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib.Models
{
	public class GeolocationCoordinatesModel
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double? Altitude { get; set; }
		public double Accuracy { get; set; }
		public double? AltitudeAccuracy { get; set; }
		public double? Heading { get; set; }
		public double? Speed { get; set; }
	}
}
