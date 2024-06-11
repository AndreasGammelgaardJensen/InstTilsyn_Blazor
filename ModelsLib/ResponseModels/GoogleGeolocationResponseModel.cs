using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ModelsLib.ResponseModels
{
	public class GoogleGeolocationResponseModel
	{
		[JsonProperty("results")]
		public List<Result> Results { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }
	}

	public class AddressComponent
	{
		[JsonProperty("long_name")]
		public string LongName { get; set; }

		[JsonProperty("short_name")]
		public string ShortName { get; set; }

		[JsonProperty("types")]
		public List<string> Types { get; set; }
	}

	public class Location
	{
		[JsonProperty("lat")]
		public double Lat { get; set; }

		[JsonProperty("lng")]
		public double Lng { get; set; }
	}

	public class Viewport
	{
		[JsonProperty("northeast")]
		public Location Northeast { get; set; }

		[JsonProperty("southwest")]
		public Location Southwest { get; set; }
	}

	public class Geometry
	{
		[JsonProperty("location")]
		public Location Location { get; set; }

		[JsonProperty("location_type")]
		public string LocationType { get; set; }

		[JsonProperty("viewport")]
		public Viewport Viewport { get; set; }

		[JsonProperty("bounds")]
		public Viewport Bounds { get; set; }
	}

	public class PlusCode
	{
		[JsonProperty("compound_code")]
		public string CompoundCode { get; set; }

		[JsonProperty("global_code")]
		public string GlobalCode { get; set; }
	}

	public class Result
	{
		[JsonProperty("address_components")]
		public List<AddressComponent> AddressComponents { get; set; }

		[JsonProperty("formatted_address")]
		public string FormattedAddress { get; set; }

		[JsonProperty("geometry")]
		public Geometry Geometry { get; set; }

		[JsonProperty("partial_match")]
		public bool PartialMatch { get; set; }

		[JsonProperty("place_id")]
		public string PlaceId { get; set; }

		[JsonProperty("plus_code")]
		public PlusCode PlusCode { get; set; }

		[JsonProperty("types")]
		public List<string> Types { get; set; }
	}
}
