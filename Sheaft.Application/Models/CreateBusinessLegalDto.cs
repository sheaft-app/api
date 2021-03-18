using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class CreateBusinessLegalDto
    {
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public AddressDto Address { get; set; }
        public CreateOwnerDto Owner { get; set; }
    }
}