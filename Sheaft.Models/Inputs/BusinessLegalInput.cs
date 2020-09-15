using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class BusinessLegalInput
    {
        public string Email { get; set; }
        public LegalKind Kind { get; set; }
        public OwnerInput Owner { get; set; }
        public AddressInput Address { get; set; }

    }
}