using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class BusinessLegalInputDto
    {
        public RegistrationKind? RegistrationKind { get; set; }
        public string RegistrationCity { get; set; }
        public string RegistrationCode { get; set; }
        public LegalKind LegalKind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public AddressDto Address { get; set; }
        public OwnerInputDto Owner { get; set; }
    }
}