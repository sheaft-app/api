﻿using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{

    public class BusinessLegalDto : LegalDto
    {
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public AddressDto Address { get; set; }
        public DeclarationDto UboDeclaration { get; set; }
    }
}