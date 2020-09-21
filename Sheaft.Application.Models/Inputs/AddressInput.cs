using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{
    public class AddressInput
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}