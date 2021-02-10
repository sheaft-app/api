using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{
    public class BirthAddressDto
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}