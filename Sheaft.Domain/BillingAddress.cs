using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class BillingAddress : Address
    {
        protected BillingAddress()
        {
        }

        public BillingAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, string name) :
            base(line1, line2, zipcode, city, country)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}