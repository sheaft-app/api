using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class BaseLegalDto
    {
        public Guid Id { get; set; }
        public OwnerDto Owner { get; set; }
    }

    public class ConsumerLegalDto : BaseLegalDto
    {
    }

    public class BusinessLegalDto : BaseLegalDto
    {
        public LegalKind Kind { get; set; }
        public string Email { get; set; }
        public AddressDto Address { get; set; }
        public UboDeclarationDto UboDeclaration { get; set; }
    }
}