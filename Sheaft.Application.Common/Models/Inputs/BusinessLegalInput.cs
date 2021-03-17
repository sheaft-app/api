using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
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