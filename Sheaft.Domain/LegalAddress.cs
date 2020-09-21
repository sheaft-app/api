using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{
    public class LegalAddress : BaseAddress
    {
        protected LegalAddress()
        {
        }

        public LegalAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country) :
            base(line1, line2, zipcode, city, country)
        {
        }
    }
}