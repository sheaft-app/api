using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{

    public class BirthAddressInput
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}