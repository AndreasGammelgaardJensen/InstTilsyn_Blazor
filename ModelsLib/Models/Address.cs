using System.Xml.Linq;

namespace ModelsLib.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Vej { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public int Zip_code { get; set; }


        public static Address ConvertStringToAddress(string addressString)
        {
            try
            {
                var separated = addressString.Split(",");

                var roadAndAddress = separated.First();
                var zipCode = separated.Last();

                var roadAndAddressList = roadAndAddress.Split(" ").ToList();

                var number = roadAndAddressList.Last();
                var lastIndex = roadAndAddressList.IndexOf(roadAndAddressList.Last());

                roadAndAddressList.RemoveAt(lastIndex);

                var roadAddress = string.Join(" ", roadAndAddressList);

                zipCode = zipCode.Trim();
                var zipCodeList = zipCode.Split(" ").ToList();

                var code = zipCodeList.First();
                var codeIndex = zipCodeList.IndexOf(code);
                var codeInt = int.Parse(zipCodeList.First());

                zipCodeList.RemoveAt(codeIndex);

                var city = string.Join(" ", zipCodeList);


                return new Address
                {
                    City = city,
                    Vej = roadAddress,
                    Number = number,
                    Zip_code = codeInt,
                };
            }
            catch ( Exception ex)
            {
                Console.WriteLine ($"Could't convert address string: {addressString}");
                return null;
            }
        }

        public string ToString()
        {

            return string.Format("ID: {0}\nVej: {1}\nNumber: {2}\nCity: {3}\nZipCode: {4}", Id, Vej,Number,City,Zip_code);
        
        }

        public string AddressToString()
        {
            return $"{Number} {Vej}, {City} {Zip_code}";
        }
    }
}