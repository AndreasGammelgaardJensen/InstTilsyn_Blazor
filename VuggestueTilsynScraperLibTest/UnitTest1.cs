using ModelsLib.Models;
using System;
using System.Net.Sockets;
using Xunit;
using static System.Net.WebRequestMethods;

namespace VuggestueTilsynScraperLibTest
{
    public class UnitTest1
    {
        [Fact]
        public void GetFiletypeTest()
        {
            var expected = "pdf";

            var fileType = "https://iwtilsynpdf.kk.dk/pdf/1875.pdf";
            var urlList = fileType.Split(".");

            var type = urlList.Last();

            Assert.Equal(expected, type);
        }


        [Fact]
        public void SeparateAddressStringIntoAddressObjectTest()
        {
            var addressString = "Tibirkegade 19, 2200 København N";

            var expected = new Address {
                Vej = "Tibirkegade",
                City = "København N",
                Zip_code = 2200,
                Number = "19"

            };

            var separated = addressString.Split(",");

            var roadAndAddress = separated.First();
            var zipCode = separated.Last();

            var roadAndAddressList = roadAndAddress.Split(" ").ToList();

            var number = roadAndAddressList.Last();
            var lastIndex = roadAndAddressList.IndexOf(roadAndAddressList.Last());

            roadAndAddressList.RemoveAt(lastIndex);

            var roadAddress  = string.Join(" ", roadAndAddressList);

            zipCode = zipCode.Trim();
            var zipCodeList = zipCode.Split(" ").ToList();

            var code = zipCodeList.First();
            var codeIndex = zipCodeList.IndexOf(code);
            var codeInt = int.Parse(zipCodeList.First());

            zipCodeList.RemoveAt(codeIndex);

            var city = string.Join(" ", zipCodeList);


            var addressObject = new Address
            {
                City = city,
                Vej = roadAddress,
                Number = number,
                Zip_code = codeInt,
            };

            Assert.Equal(expected.Vej, addressObject.Vej);
            Assert.Equal(expected.City, addressObject.City);
            Assert.Equal(expected.Number, addressObject.Number);
            Assert.Equal(expected.Zip_code, addressObject.Zip_code);

        }
    }
}