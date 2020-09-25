using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{
    public class BirthAddress
    {
        protected BirthAddress()
        {
        }

        public BirthAddress(string city, CountryIsoCode country)
        {
            City = city;
            Country = country;
        }

        public string City { get; private set; }
        public CountryIsoCode Country { get; private set; }
    }
}