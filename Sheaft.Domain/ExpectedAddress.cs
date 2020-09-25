using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{
    public class ExpectedAddress : Address
    {
        protected ExpectedAddress()
        {
        }

        public ExpectedAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, double? longitude = null, double? latitude = null)
            : base(line1, line2, zipcode, city, country)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
    }
}