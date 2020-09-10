using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class DeliveryAddress : LocationAddress
    {
        protected DeliveryAddress()
        {
        }

        public DeliveryAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, double? longitude, double? latitude) :
            base(line1, line2, zipcode, city, country, longitude, latitude)
        {
        }
    }
}