using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public abstract class Address
    {
        protected Address()
        {
        }

        protected Address(string line1, string line2, string zipcode, string city, CountryIsoCode country)
        {
            if (string.IsNullOrWhiteSpace(line1))
                throw new ValidationException(MessageKind.Address_Line1_Required);

            if (string.IsNullOrWhiteSpace(zipcode))
                throw new ValidationException(MessageKind.Address_Zipcode_Required);

            if (string.IsNullOrWhiteSpace(city))
                throw new ValidationException(MessageKind.Address_City_Required);

            Line1 = line1;
            Line2 = line2;
            City = city;
            Country = country;

            if(zipcode.Length == 5)
                Zipcode = zipcode;
        }

        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string Zipcode { get; private set; }
        public string City { get; private set; }
        public CountryIsoCode Country { get; private set; }
    }
}