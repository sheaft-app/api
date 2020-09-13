using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class BankAddress : BaseAddress
    {
        protected BankAddress()
        {
        }

        public BankAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country) 
            : base(line1, line2, zipcode, city, country)
        {
        }
    }
}