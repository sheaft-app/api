using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class LegalAddress
    {
        protected LegalAddress()
        {
        }

        public LegalAddress(string line1, string line2, string zipcode, string city, string country)
        {
            if (string.IsNullOrWhiteSpace(line1))
                throw new ValidationException(MessageKind.Address_Line1_Required);

            if (string.IsNullOrWhiteSpace(zipcode))
                throw new ValidationException(MessageKind.Address_Zipcode_Required);

            if (string.IsNullOrWhiteSpace(city))
                throw new ValidationException(MessageKind.Address_City_Required);

            Line1 = line1;
            Line2 = line2;
            Zipcode = zipcode;
            City = city;
            Country = country;
        }

        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string Zipcode { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
    }
}