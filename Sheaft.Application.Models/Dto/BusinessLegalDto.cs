using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class BusinessLegalDto : BaseLegalDto
    {
        public LegalKind Kind { get; set; }
        public string Email { get; set; }
        public AddressDto Address { get; set; }
        public UboDeclarationDto UboDeclaration { get; set; }
    }
}