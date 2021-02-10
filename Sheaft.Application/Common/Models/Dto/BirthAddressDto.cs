using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class BirthAddressDto
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}