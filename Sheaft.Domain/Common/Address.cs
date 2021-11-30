using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain.BaseClass
{
    public class Address
    {
        protected Address()
        {
        }

        protected Address(string line1, string line2, string zipcode, string city, CountryIsoCode country)
        {
            if (string.IsNullOrWhiteSpace(line1))
                throw new ValidationException("La ligne d'adresse est requise.");

            if (string.IsNullOrWhiteSpace(zipcode))
                throw new ValidationException("Le code postal est requis.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ValidationException("La ville est requise.");
            
            Line1 = line1;
            Line2 = line2;
            City = city;
            Country = country;

            if(zipcode.Length == 5)
                Zipcode = zipcode;
        }

        public string Line1 { get; }
        public string Line2 { get; }
        public string Zipcode { get; }
        public string City { get; }
        public string Department { get; }
        public string Region { get; }
        public CountryIsoCode Country { get; }
        
        public override string ToString()
        {
            return $"{Line1}\n{(!string.IsNullOrWhiteSpace(Line2) ? Line2 + "\n" : "")}{Zipcode} {City}\n";
        }
    }
}