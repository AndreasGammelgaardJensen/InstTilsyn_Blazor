using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeolocationLib.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLib.ResponseModels;
using Newtonsoft.Json;
using ModelsLib.Models;

namespace GeolocationLib.Google.Tests
{
	[TestClass()]
	public class LocationMapperTests
	{
		[TestMethod()]
		public void MapKoordinatesToInstitutionsTest()
		{
			var testString = @"{
    ""results"": [
        {
            ""address_components"": [
                {
                    ""long_name"": ""13"",
                    ""short_name"": ""13"",
                    ""types"": [
                        ""street_number""
                    ]
                },
                {
                    ""long_name"": ""Kløverbakkevej"",
                    ""short_name"": ""Kløverbakkevej"",
                    ""types"": [
                        ""route""
                    ]
                },
                {
                    ""long_name"": ""Odder"",
                    ""short_name"": ""Odder"",
                    ""types"": [
                        ""locality"",
                        ""political""
                    ]
                },
                {
                    ""long_name"": ""Denmark"",
                    ""short_name"": ""DK"",
                    ""types"": [
                        ""country"",
                        ""political""
                    ]
                },
                {
                    ""long_name"": ""8300"",
                    ""short_name"": ""8300"",
                    ""types"": [
                        ""postal_code""
                    ]
                }
            ],
            ""formatted_address"": ""Kløverbakkevej 13, 8300 Odder, Denmark"",
            ""geometry"": {
                ""location"": {
                    ""lat"": 55.9853742,
                    ""lng"": 10.1256068
                },
                ""location_type"": ""ROOFTOP"",
                ""viewport"": {
                    ""northeast"": {
                        ""lat"": 55.98667273029149,
                        ""lng"": 10.1269683302915
                    },
                    ""southwest"": {
                        ""lat"": 55.9839747697085,
                        ""lng"": 10.1242703697085
                    }
                }
            },
            ""partial_match"": true,
            ""place_id"": ""ChIJkSmQkb5CTEYRQyTFZwcxTbA"",
            ""plus_code"": {
                ""compound_code"": ""X4PG+46 Odder, Denmark"",
                ""global_code"": ""9F7GX4PG+46""
            },
            ""types"": [
                ""street_address""
            ]
        },
        {
            ""address_components"": [
                {
                    ""long_name"": ""20"",
                    ""short_name"": ""20"",
                    ""types"": [
                        ""street_number""
                    ]
                },
                {
                    ""long_name"": ""Klostervej"",
                    ""short_name"": ""Klostervej"",
                    ""types"": [
                        ""route""
                    ]
                },
                {
                    ""long_name"": ""Ry"",
                    ""short_name"": ""Ry"",
                    ""types"": [
                        ""locality"",
                        ""political""
                    ]
                },
                {
                    ""long_name"": ""Denmark"",
                    ""short_name"": ""DK"",
                    ""types"": [
                        ""country"",
                        ""political""
                    ]
                },
                {
                    ""long_name"": ""8680"",
                    ""short_name"": ""8680"",
                    ""types"": [
                        ""postal_code""
                    ]
                }
            ],
            ""formatted_address"": ""Klostervej 20, 8680 Ry, Denmark"",
            ""geometry"": {
                ""bounds"": {
                    ""northeast"": {
                        ""lat"": 56.0913191,
                        ""lng"": 9.758133299999999
                    },
                    ""southwest"": {
                        ""lat"": 56.0911992,
                        ""lng"": 9.757906199999999
                    }
                },
                ""location"": {
                    ""lat"": 56.0912585,
                    ""lng"": 9.758020799999999
                },
                ""location_type"": ""ROOFTOP"",
                ""viewport"": {
                    ""northeast"": {
                        ""lat"": 56.0926081302915,
                        ""lng"": 9.759371080291501
                    },
                    ""southwest"": {
                        ""lat"": 56.0899101697085,
                        ""lng"": 9.756673119708497
                    }
                }
            },
            ""partial_match"": true,
            ""place_id"": ""ChIJg2rPEvxzTEYR7Hz45ocYxVg"",
            ""types"": [
                ""premise""
            ]
        }
    ],
    ""status"": ""OK""
        }";


			GoogleGeolocationResponseModel response = JsonConvert.DeserializeObject<GoogleGeolocationResponseModel>(testString);

            var instList = new List<InstitutionFrontPageModel>();

            instList.Add(new InstitutionFrontPageModel
            {
                Koordinates = new InstKoordinates() { Id = Guid.Empty },
                address = new Address { Id = Guid.Empty, Vej = "Kløverbakkevej", Number = "13" }
            }
                );

			instList.Add(new InstitutionFrontPageModel
			{
				Koordinates = new InstKoordinates() { Id = Guid.Empty },
				address = new Address { Id = Guid.Empty, Vej = "Klostervej", Number = "20" }
			}
		);
            if ( response != null )
            {
				instList = LocationMapper.MapKoordinatesToInstitutions(response.Results, instList);


			}

            instList.ForEach(inst =>
            {
                if(inst.address.Vej == "Kløverbakkevej")
                {
					Assert.IsTrue(inst.Koordinates.lat is not null);
					Assert.IsTrue(inst.Koordinates.lng is not null);
					Assert.AreEqual(inst.Koordinates.lat, response.Results[0].Geometry.Location.Lat);
					Assert.AreEqual(inst.Koordinates.lng, response.Results[0].Geometry.Location.Lng);
				}

                else if (inst.address.Vej == "Klostervej")
                {
				
					Assert.IsTrue(inst.Koordinates.lat is not null);
					Assert.IsTrue(inst.Koordinates.lng is not null);
					Assert.AreEqual(inst.Koordinates.lat, response.Results[1].Geometry.Location.Lat);
					Assert.AreEqual(inst.Koordinates.lng, response.Results[1].Geometry.Location.Lng);
				
				}
                

            });
		}
	}
}