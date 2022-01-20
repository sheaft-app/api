using NetTopologySuite.Geometries;
using Sheaft.Domain.BaseClass;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class ShippingAddress : Address
    {
        protected ShippingAddress()
        {
        }

        internal ShippingAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, double? longitude, double? latitude) 
            : base(line1, line2, zipcode, city, country)
        {
            Longitude = longitude;
            Latitude = latitude;

            if (latitude.HasValue && longitude.HasValue)
                Location = LocationProvider.CreatePoint(latitude.Value, longitude.Value);
        }

        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
        public Point Location { get; private set; }
    }
}