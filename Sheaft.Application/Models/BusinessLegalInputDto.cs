﻿using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class BusinessLegalInputDto
    {
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public AddressDto Address { get; set; }
        public OwnerInputDto Owner { get; set; }
    }
}