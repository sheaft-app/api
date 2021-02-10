using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
{

    public class BirthAddressInput
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}