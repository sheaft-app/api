using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public abstract class Address
    {
        protected Address()
        {
        }

        protected Address(string line1, string line2, string zipcode, string city, CountryIsoCode country)
        {
            if (string.IsNullOrWhiteSpace(line1))
                throw SheaftException.Validation("La ligne d'adresse est requise.");

            if (string.IsNullOrWhiteSpace(zipcode))
                throw SheaftException.Validation("Le code postal est requis.");

            if (string.IsNullOrWhiteSpace(city))
                throw SheaftException.Validation("La ville est requise.");
            
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
        
        public override string ToString()
        {
            return $"{Line1}\n{(!string.IsNullOrWhiteSpace(Line2) ? Line2 + "\n" : "")}{Zipcode} {City}";
        }
    }
}