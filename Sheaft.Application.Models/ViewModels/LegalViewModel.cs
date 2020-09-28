using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class LegalViewModel
    {
        public Guid Id { get; set; }
        public LegalKind Kind { get; set; }
        public OwnerViewModel Owner { get; set; }
    }
}
