using Sheaft.Interop.Enums;

namespace Sheaft.Models.Inputs
{

    public class BirthAddressInput
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}