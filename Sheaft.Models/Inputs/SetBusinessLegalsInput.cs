using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Models.Inputs
{
    public class SetBusinessLegalsInput
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public LegalKind Kind { get; set; }
        public OwnerInput Owner { get; set; }
        public AddressInput Address { get; set; }
    }
}