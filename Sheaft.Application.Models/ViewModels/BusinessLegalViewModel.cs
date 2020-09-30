using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class BusinessLegalViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
        public OwnerViewModel Owner { get; set; }
        public AddressViewModel Address { get; set; }
        public UboDeclarationViewModel UboDeclaration { get; set; }
    }
}
