using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class UboAddress : BaseAddress
    {
        protected UboAddress()
        {
        }

        public UboAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country)
            : base(line1, line2, zipcode, city, country)
        {
        }
    }
}