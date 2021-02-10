using Sheaft.Domain.Enums;

namespace Sheaft.Application.Models
{
    public class BusinessLegalInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public LegalKind Kind { get; set; }
        public OwnerInput Owner { get; set; }
        public AddressInput Address { get; set; }
    }
}