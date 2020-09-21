using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class BusinessLegalInput
    {
        public string Email { get; set; }
        public LegalKind Kind { get; set; }
        public OwnerInput Owner { get; set; }
        public AddressInput Address { get; set; }

    }
}