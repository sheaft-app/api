using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class LegalViewModel
    {
        public Guid Id { get; set; }
        public LegalKind Kind { get; set; }
        public LegalValidation Validation { get; set; }
        public OwnerViewModel Owner { get; set; }
        public IEnumerable<DocumentViewModel> Documents { get; set; }
    }
}
