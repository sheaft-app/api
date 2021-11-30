using Sheaft.Domain.BaseClass;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class LegalsAddress : Address
    {
        protected LegalsAddress()
        {
        }

        public LegalsAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country) :
            base(line1, line2, zipcode, city, country)
        {
        }
    }
}