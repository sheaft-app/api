using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{

    public class BillingAddress : Address
    {
        protected BillingAddress()
        {
        }

        public BillingAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country):
            base(line1, line2,zipcode, city, country)
        {
        }
    }
}